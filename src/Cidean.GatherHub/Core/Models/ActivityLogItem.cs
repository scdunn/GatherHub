using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class ActivityLogItem
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Message { get; set; }
    }
}
