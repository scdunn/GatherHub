// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents a Category classification for a course.
    /// </summary>
    public class CourseCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of CourseCategory.
        /// </summary>
        public CourseCategory() { }
        
        /// <summary>
        /// Initializes a new instance of CourseCategory with category title.
        /// </summary>
        /// <param name="title"></param>
        public CourseCategory(string title)
        {
            Title = title;
        }
    }
}
