// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using Cidean.GatherHub.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents an individual course to which members can register.
    /// </summary>
    public class Course
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        
        public string Location { get; set; }

       
        public string MeetingTimes { get; set; }

        [Required]
        public int CourseCategoryId { get; set; }
        public virtual CourseCategory Category { get; set; }

        public int InstructorId { get; set; }
        public virtual Member Instructor { get; set; }

        [MaxLength(256)]
        public string ImageUrl { get; set; }

        public string ShortDescription {
            get {
                //generate a non-formatted shorter version of
                //description from html formatted description.
                var shortDesc = Description.StripHtml();

                if (shortDesc.Length > 256)
                    return shortDesc.Substring(0, 256);
                return shortDesc;

            } }

        public virtual ICollection<CourseMember> CourseMembers { get; set; }


        /// <summary>
        /// Initializes a new instance of course.
        /// </summary>
        public Course()
        {
            CourseMembers = new List<CourseMember>();
        }

        /// <summary>
        /// Initializes a new instance of course from values.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="meetingTimes"></param>
        /// <param name="courseCategoryId"></param>
        /// <param name="instructorId"></param>
        public Course(string title, string description, string location, string meetingTimes, int courseCategoryId, int instructorId):this()
        {
            Title = title;
            Description = description;
            Location = location;
            MeetingTimes = meetingTimes;
            CourseCategoryId = courseCategoryId;
            InstructorId = instructorId;
        }

        /// <summary>
        /// Add a new member to the course given their member id.
        /// </summary>
        /// <param name="memberId"></param>
        public void AddMember(int memberId)
        {
            CourseMembers.Add(new CourseMember() { CourseId = Id, MemberId = memberId });
        }



    }
}
