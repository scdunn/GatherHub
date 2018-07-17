// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents a Member registered for a course (Bridge object/table).
    /// </summary>
    public class CourseMember
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }

    }


  
}
