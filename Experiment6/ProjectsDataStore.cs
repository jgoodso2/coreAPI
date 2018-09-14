using ProjectInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API
{
    public class ProjectsDataStore
    {
        public static ProjectsDataStore Current { get; } = new ProjectsDataStore();
        public List<ProjectDto> Projects { get; set; }

        public ProjectsDataStore()
        {
            // init dummy data
            Projects = new List<ProjectDto>()
            {
                new ProjectDto()
                {
                     ProjectGuid = Guid.NewGuid(),
                     ProjectName = "New York Project",
                     PlanViewProjects = new List<PlanViewProjectDto>()
                     {
                         new PlanViewProjectDto() {
                             ppl_Code = "123",
                             ProjectGuid = Guid.NewGuid(),
                             ProjectName = "Central Park",
                             //Description = "The most visited urban park in the United States." 
                         },
                          new PlanViewProjectDto() {
                             ppl_Code = "456",
                             ProjectGuid = Guid.NewGuid(),
                             ProjectName = "Empire State Building",
                              //Description = "A 102-story skyscraper located in Midtown Manhattan."
                          },
                     }
                }
            };

        }
    }
}
