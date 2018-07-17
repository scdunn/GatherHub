// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

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
    /// Represents a unique member which can sign in, register and teach course.
    /// </summary>
    public class Member
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string FirstName { get; set; }

        [Required, MaxLength(30)]
        public string LastName { get; set; }

        [Required, EmailAddress, MaxLength(255) ]
        public string EmailAddress { get; set; }

        private string Password { get; set; }
        public string TempPassword { get; set; }

        /// <summary>
        /// Returns join of First and Last name properties.
        /// </summary>
        public string FullName { get { return FirstName + " " + LastName; } }


        /// <summary>
        /// Returns list of Courses a member is registered for.
        /// </summary>
        public ICollection<CourseMember> CourseMembers { get; set; }

        /// <summary>
        /// Initializes a new instance of Member.
        /// </summary>
        public Member()
        {
            CourseMembers = new List<CourseMember>();
        }
        

        /// <summary>
        /// Initializes a new instance of Member with initial property values.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        public Member(string emailAddress, string firstName, string lastName, string password):this()
        {
            EmailAddress = emailAddress;
            FirstName = firstName;
            LastName = lastName;
            SetPassword(password);
        }

        /// <summary>
        /// Adds a new Course to list of Members registered Courses.
        /// </summary>
        /// <param name="courseId"></param>
        public void AddCourse(int courseId)
        {
            CourseMembers.Add(new CourseMember() { MemberId = Id, CourseId = courseId });
        }

        /// <summary>
        /// Set hashed value of password from clear text.
        /// </summary>
        /// <param name="value"></param>
        public void SetPassword(string value)
        {
            //TODO: Set iterations somewhere else.
            Password = Helpers.Hasher.Generate(value, 100);
        }

        /// <summary>
        /// Returns true if hashed value equals existing password hash.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValidPassword(string value)
        {
            if (Helpers.Hasher.IsValid(value, Password))
                return true;
            return false;
        }

        public class MemberConfig : IEntityTypeConfiguration<Member>
        {
            public void Configure(EntityTypeBuilder<Member> builder)
            {
                builder.Property(u => u.Password);
            }
        }
    }
}
