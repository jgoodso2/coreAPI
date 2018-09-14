using ProjectInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Services
{
    public interface IProjectInfoRepository
    {
        bool ProjectExists(Guid projectUid);
        IEnumerable<Project> GetProjects();
        Project GetProject(Guid projectUid, bool includePlanViewProjects);
        IEnumerable<PlanViewProject> GetPlanViewProjectsForProject(Guid projectUid);
        PlanViewProject GetPlanViewProjectForProject(Guid projectUid, string ppl_code);
        void AddPlanViewProjectForProject(Guid projectUid, PlanViewProject PlanViewProject);
        void AddProject(Project PlanViewProject);
        void DeletePlanViewProject(PlanViewProject PlanViewProject);
        void DeleteProject(Project Project);

        IEnumerable<AuthorizedPlanViewProject> GetAuthorizedPlanViewProjects();
        bool Save();
    }
}
