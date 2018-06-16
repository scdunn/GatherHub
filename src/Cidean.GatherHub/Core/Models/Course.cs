using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string MeetingTimes { get; set; }
        public int CourseCategoryId { get; set; }
        public virtual CourseCategory Category { get; set; }

    }
}
