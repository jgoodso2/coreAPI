using AutoMapper;
using ProjectInfo.API.Models;
using ProjectInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Controllers
{
    [Route("api/Projects")]
    [RequireHttpsAttribute]
    public class PlanViewProjectsController : Controller
    {
        private ILogger<PlanViewProjectsController> _logger;
        //private IMailService _mailService;
        private IProjectInfoRepository _ProjectInfoRepository;


        public PlanViewProjectsController(ILogger<PlanViewProjectsController> logger,
            //IMailService mailService,
            IProjectInfoRepository ProjectInfoRepository)
        {
            _logger = logger;
            //_mailService = mailService;
            _ProjectInfoRepository = ProjectInfoRepository;
        }

        [HttpGet("Admin/AuthorizedPlanViewProjects")]
        public IActionResult GetAuthorizedPlanViewProjects()
        {
            var authorizedProjectEntities = _ProjectInfoRepository.GetAuthorizedPlanViewProjects();
            var results = Mapper.Map<IEnumerable<AuthorizedPlanViewProjectDto>>(authorizedProjectEntities);

            return Ok(results);
        }

        [HttpGet("{projectUid}/PlanViewProjects")]
        public IActionResult GetPlanViewProjects(Guid projectUid)
        {
            try
            {
                if (!_ProjectInfoRepository.ProjectExists(projectUid))
                {
                    _logger.LogInformation($"Project with id {projectUid} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var PlanViewProjectsForProject = _ProjectInfoRepository.GetPlanViewProjectsForProject(projectUid);
                var PlanViewProjectsForProjectResults =
                                   Mapper.Map<IEnumerable<PlanViewProjectDto>>(PlanViewProjectsForProject);

                return Ok(PlanViewProjectsForProjectResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for Project with id {projectUid}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{projectUid}/PlanViewProjects/{ppl_code}", Name = "GetPlanViewProject")]
        public IActionResult GetPlanViewProject(Guid projectUid, string ppl_code)
        {
            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var PlanViewProject = _ProjectInfoRepository.GetPlanViewProjectForProject(projectUid, ppl_code);

            if (PlanViewProject == null)
            {
                return NotFound();
            }

            var PlanViewProjectResult = Mapper.Map<PlanViewProjectDto>(PlanViewProject);
            return Ok(PlanViewProjectResult);
        }

        [HttpPost("{projectUid}/PlanViewProjects")]
        public IActionResult CreatePlanViewProject(Guid projectUid,
            [FromBody] PlanViewProjectsForCreationDto PlanViewProject)

        {
            if (PlanViewProject == null)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var finalPlanViewProject = Mapper.Map<Entities.PlanViewProject>(PlanViewProject);

            _ProjectInfoRepository.AddPlanViewProjectForProject(projectUid, finalPlanViewProject);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdPlanViewProjectToReturn = Mapper.Map<Models.PlanViewProjectDto>(finalPlanViewProject);     ////////////

            return CreatedAtRoute("GetPlanViewProject", new
            { projectUid = projectUid, ppl_code = createdPlanViewProjectToReturn.ppl_Code }, createdPlanViewProjectToReturn);
        }

        [HttpPut("{projectUid}/PlanViewProjects/{ppl_code}")]
        public IActionResult UpdatePlanViewProject(Guid projectUid, string ppl_code,
            [FromBody] PlanViewProjectsForUpdateDto PlanViewProject)
        {
            if (PlanViewProject == null)
            {
                return BadRequest();
            }



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var PlanViewProjectEntity = _ProjectInfoRepository.GetPlanViewProjectForProject(projectUid, ppl_code);
            if (PlanViewProjectEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(PlanViewProject, PlanViewProjectEntity);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }


        [HttpPatch("{projectUid}/PlanViewProjects/{ppl_code}")]
        public IActionResult PartiallyUpdatePlanViewProject(Guid projectUid, string ppl_code,
            [FromBody] JsonPatchDocument<PlanViewProjectsForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var PlanViewProjectEntity = _ProjectInfoRepository.GetPlanViewProjectForProject(projectUid, ppl_code);
            if (PlanViewProjectEntity == null)
            {
                return NotFound();
            }

            var PlanViewProjectToPatch = Mapper.Map<PlanViewProjectsForUpdateDto>(PlanViewProjectEntity);

            patchDoc.ApplyTo(PlanViewProjectToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            TryValidateModel(PlanViewProjectToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(PlanViewProjectToPatch, PlanViewProjectEntity);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{projectUid}/PlanViewProjects/{ppl_code}")]
        public IActionResult DeletePlanViewProject(Guid projectUid, string ppl_code)
        {
            if (!_ProjectInfoRepository.ProjectExists(projectUid))
            {
                return NotFound();
            }

            var PlanViewProjectEntity = _ProjectInfoRepository.GetPlanViewProjectForProject(projectUid, ppl_code);
            if (PlanViewProjectEntity == null)
            {
                return NotFound();
            }

            _ProjectInfoRepository.DeletePlanViewProject(PlanViewProjectEntity);

            if (!_ProjectInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

           // _mailService.Send("Point of interest deleted.",
            //        $"Point of interest {PlanViewProjectEntity.ProjectName} with id {PlanViewProjectEntity.ppl_Code} was deleted.");

            return NoContent();
        }
    }
}
