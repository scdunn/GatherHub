using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Core.Data
{
    public class ActivityContext:DbContext
    {
        public ActivityContext(DbContextOptions<ActivityContext> options)
        : base(options)
        {
        }

       
        public DbSet<ActivityLogItem> ActivityLogItems { get; set; }

        


    }

}
