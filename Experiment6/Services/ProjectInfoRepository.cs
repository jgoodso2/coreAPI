using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectInfo.API.Services
{
    public class ProjectInfoRepository : IProjectInfoRepository
    {
        private ProjectContext _context;
        public ProjectInfoRepository(ProjectContext context)
        {
            _context = context;
        }
        public void AddProject(Project project)
        {
            _context.Projects.Add(project);
        }

        public void AddPlanViewProjectForProject(Guid projectUid, PlanViewProject PlanViewProject)
        {
            var Project = GetProject(projectUid, false);
            Project.PlanViewProjects.Add(PlanViewProject);
        }

        public bool ProjectExists(Guid projectUid)
        {
            return _context.Projects.Any(c => c.ProjectGuid == projectUid);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects.OrderBy(c => c.ProjectName).ToList();
        }

        public Project GetProject(Guid projectUId, bool includePlanViewProjects)
        {
            if (includePlanViewProjects)
            {
                return _context.Projects.Include(c => c.PlanViewProjects)   //////////////////////////////////////////////
                    .Where(c => c.ProjectGuid == projectUId).FirstOrDefault();
            }

            return _context.Projects.Where(c => c.ProjectGuid == projectUId).FirstOrDefault();
        }

        public PlanViewProject GetPlanViewProjectForProject(Guid projectUid, string ppl_code)
        {
            return _context.PlanViewProjects
               .Where(p => p.ProjectGuid == projectUid && p.ppl_Code == ppl_code).FirstOrDefault();
        }

        public IEnumerable<PlanViewProject> GetPlanViewProjectsForProject(Guid projectUid)
        {
            return _context.PlanViewProjects
                           .Where(p => p.ProjectGuid == projectUid).ToList();
        }

        public void DeletePlanViewProject(PlanViewProject PlanViewProject)
        {
            _context.PlanViewProjects.Remove(PlanViewProject);
        }

        public void DeleteProject(Project Project)
        {
            foreach (var planViewProject in Project.PlanViewProjects)
            {
                DeletePlanViewProject(planViewProject);
            }
            _context.Projects.Remove(Project);
        }

        public IEnumerable<AuthorizedPlanViewProject> GetAuthorizedPlanViewProjects()
        {
            return _context.AuthorizedPlanViewProjects.ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
