using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Objectives
{
    [Flags]
    public enum ObjectiveScope
    {
        EXPLORATION=1,
        EXPERIMENTATION=2,
        EXAMINATION=4,
        EXERCISE=8
    }
}
