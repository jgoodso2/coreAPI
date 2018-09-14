using AutoMapper;
using ProjectInfo.API.Models;
using ProjectInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Controllers
{
    [Route("api/Projects")]
    [RequireHttpsAttribute]
    public class ProjectsController : Controller
    {
        private IProjectInfoRepository _ProjectInfoRepository;

        public ProjectsController(IProjectInfoRepository ProjectInfoRepository)
        {
            _ProjectInfoRepository = ProjectInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetProjects()
        {
            var ProjectEntities = _ProjectInfoRepository.GetProjects();
            var results = Mapper.Map<IEnumerable<ProjectWithoutPlanViewProjectsDto>>(ProjectEntities);

            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetProject")]
        public IActionResult GetProject(Guid id, bool includePlanViewProjects = false)
        {
            var Project = _ProjectInfoRepository.GetProject(id, includePlanViewProjects);

            if (Project == null)
            {
                return NotFound();
            }

            if (includePlanViewProjects)
            {
                var ProjectResult = Mapper.Map<ProjectDto>(Project);
                return Ok(ProjectResult);
            }

            var ProjectWithoutPlanViewProjectsResult = Mapper.Map<ProjectWithoutPlanViewProjectsDto>(Project);
            return Ok(ProjectWithoutPlanViewProjectsResult);
        }

        [HttpPost()]
        public IActionResult CreateProject(
           [FromBody] ProjectDto project)

        {
            if (project == null)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_ProjectInfoRepository.ProjectExists(project.ProjectGuid))
            {
                return BadRequest("A Project with the same projectUid already exists");
            }

            var finalProject = Mapper.Map<Entities.Project>(project);

            _ProjectInfoRepository.AddProject(finalProject);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdProjectToReturn = Mapper.Map<Models.ProjectDto>(finalProject);     ////////////

            return CreatedAtRoute("GetProject", new
            { id = createdProjectToReturn.ProjectGuid }, createdProjectToReturn);
        }

        [HttpDelete("{projectUid}")]
        public IActionResult DeleteProject(Guid projectUid)
        {
            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var ProjectEntity = _ProjectInfoRepository.GetProject(projectUid, true);
            if (ProjectEntity == null)
            {
                return NotFound();
            }

            _ProjectInfoRepository.DeleteProject(ProjectEntity);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //_mailService.Send("Point of interest deleted.",
            //        $"Point of interest {ProjectEntity.ProjectName} with id {ProjectEntity.ppl_Code} was deleted.");

            return NoContent();
        }
    }
}
