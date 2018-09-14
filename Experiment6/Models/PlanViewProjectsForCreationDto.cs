using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Models
{
    public class PlanViewProjectsForCreationDto
    {
        [Required(ErrorMessage = "You should provide a project name value.")]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [MaxLength(7)]
        public string ppl_Code { get; set; }
    }
}
