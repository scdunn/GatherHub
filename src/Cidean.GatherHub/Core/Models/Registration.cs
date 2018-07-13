using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class Registration
    {
        public int MemberId { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Registration()
        {
            Courses = new List<Course>();
        }
    }
}
