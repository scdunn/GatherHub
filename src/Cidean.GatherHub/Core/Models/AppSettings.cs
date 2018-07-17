// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    /// <summary>
    /// Represents typed Application Settings from appsettings.json.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Returns true if database should be reinitialized.
        /// </summary>
        public bool ResetDatabase { get; set; }

        /// <summary>
        /// Duration in seconds when sessions should timeout if no activity.
        /// </summary>
        public int SessionTimeout { get; set; }
    }
}
