using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Models
{
    public class ProjectDto
    {
        public Guid ProjectGuid { get; set; }
        public string ProjectName { get; set; }


        public ICollection<PlanViewProjectDto> PlanViewProjects { get; set; }
        = new List<PlanViewProjectDto>();
    }
}
