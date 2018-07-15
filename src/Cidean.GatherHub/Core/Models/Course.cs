using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public int InstructorId { get; set; }
        public virtual Member Instructor { get; set; }


        public string ShortDescription {
            get {
                if (Description.Length > 256)
                    return Description.Substring(0, 256);
                return Description;

            } }

        public virtual ICollection<CourseMember> CourseMembers { get; set; }

        public Course()
        {
            CourseMembers = new List<CourseMember>();
        }

        public Course(string title, string description, string location, string meetingTimes, int courseCategoryId, int instructorId):this()
        {
            Title = title;
            Description = description;
            Location = location;
            MeetingTimes = meetingTimes;
            CourseCategoryId = courseCategoryId;
            InstructorId = instructorId;
        }


        public void AddMember(int memberId)
        {
            CourseMembers.Add(new CourseMember() { CourseId = Id, MemberId = memberId });
        }

        public class CourseConfig : IEntityTypeConfiguration<Course>
        {
            public void Configure(EntityTypeBuilder<Course> builder)
            {
                builder
                    .HasOne(p => p.Instructor)
                    .WithMany()
                    .HasForeignKey(p => p.InstructorId);
                    
                    }
        }

    }
}
