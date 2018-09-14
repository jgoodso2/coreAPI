using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Entities
{
    public class PlanViewProject
    {
        [Required]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(200)]

        public string ppl_Code { get; set; }

        [ForeignKey("ProjectGuid")]
        public Project Project { get; set; }

        public Guid ProjectGuid { get; set; }
    }
}
