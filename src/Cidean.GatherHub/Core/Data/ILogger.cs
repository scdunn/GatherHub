﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public interface IActivityLogger
    {
        void Log(string message);
    }
}
