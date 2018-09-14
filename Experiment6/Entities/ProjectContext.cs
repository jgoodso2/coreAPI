using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectInfo.API.Entities
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
           : base(options)
        {
            Database.Migrate();

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<PlanViewProject> PlanViewProjects { get; set; }

        public DbSet<AuthorizedPlanViewProject> AuthorizedPlanViewProjects { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
