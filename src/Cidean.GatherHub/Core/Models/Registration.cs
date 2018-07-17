// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents a registration which allows Members to take a Course.
    /// </summary>
    public class Registration
    {
        public int MemberId { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        /// <summary>
        /// Initializes a new instance of Registration.
        /// </summary>
        public Registration()
        {
            Courses = new List<Course>();
        }

        
    }
}
