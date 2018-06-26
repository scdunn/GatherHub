using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string MeetingTimes { get; set; }
        [Required]
        public int CourseCategoryId { get; set; }
        public virtual CourseCategory Category { get; set; }

        public Course()
        {

        }

        public Course(string title, string description, string location, string meetingTimes, int courseCategoryId)
        {
            Title = title;
            Description = description;
            Location = location;
            MeetingTimes = meetingTimes;
            CourseCategoryId = courseCategoryId;
        }

    }
}
