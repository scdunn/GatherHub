using Cidean.GatherHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public class ActivityLogger : ILogger
    {
        private readonly ActivityContext _context;

        public ActivityLogger(ActivityContext context)
        {
            _context = context;
        }

        public void Log(string message)
        {
            _context.ActivityLogItems.Add(new ActivityLogItem() { ActivityDate = DateTime.Now, Message = message });
            _context.SaveChanges();
        }
    }
}
