// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents an activity to log including date and message.
    /// </summary>
    public class ActivityLogItem
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }
    }
}
