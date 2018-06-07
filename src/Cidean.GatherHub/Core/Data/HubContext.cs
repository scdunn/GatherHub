using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Core.Data
{
    public class HubContext:DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./hub.db");
        }
    }
}
