﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public interface IProxy
    {
        object GetSource();

        void Reset();
    }
}
