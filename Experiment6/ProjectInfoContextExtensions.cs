using ProjectInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API
{
    public static class ProjectInfoExtensions
    {
        public static void EnsureSeedDataForContext(this ProjectContext context)
        {
            if (context.Projects.Any())
            {
                return;
            }

            // init seed data
            var Projects = new List<Project>()
            {
                new Project()
                {
                    ProjectGuid = Guid.NewGuid(),
                    ProjectName = "New York Project",
                     //Description = "The one with that big park.",
                     PlanViewProjects = new List<PlanViewProject>()
                     {
                         new PlanViewProject() {
                             ppl_Code = "123",
                             ProjectName = "Central Park",
                             //Description = "The most visited urban park in the United States."
                         },
                          new PlanViewProject() {
                              ppl_Code = "456",
                             ProjectName = "Empire State Building",
                             //Description = "A 102-story skyscraper located in Midtown Manhattan."
                          },
                     }
                },
                new Project()
                {
                    ProjectGuid = Guid.NewGuid(),
                    ProjectName = "Antwerp",
                    //Description = "The one with the cathedral that was never really finished.",
                    PlanViewProjects = new List<PlanViewProject>()
                     {
                         new PlanViewProject() {
                             ppl_Code = "789",
                             ProjectName = "Cathedral",
                             //Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                         },
                          new PlanViewProject() {
                              ppl_Code = "678",
                             ProjectName = "Antwerp Central Station",
                             //Description = "The the finest example of railway architecture in Belgium."
                          },
                     }
                },
                new Project()
                {
                    ProjectGuid = Guid.NewGuid(),
                    ProjectName = "Paris",
                    //Description = "The one with that big tower.",
                    PlanViewProjects = new List<PlanViewProject>()
                     {
                         new PlanViewProject() {
                             ppl_Code = "abc",
                             ProjectName = "Eiffel Tower",
                             //Description =  "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                         },
                          new PlanViewProject() {
                              ppl_Code = "def",
                             ProjectName = "The Louvre",
                             //Description = "The world's largest museum."
                          },
                     }
                }
            };

            context.Projects.AddRange(Projects);
            context.SaveChanges();
        }
    }
}
