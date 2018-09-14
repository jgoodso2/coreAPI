using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ProjectGuid { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        public ICollection<PlanViewProject> PlanViewProjects { get; set; }
               = new List<PlanViewProject>();
    }
}
