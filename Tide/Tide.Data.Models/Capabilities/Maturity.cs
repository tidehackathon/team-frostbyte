using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Capabilities
{
    public enum Maturity : int
    {
        DEVELOPMENTAL = 0,
        EXPERIMENTAL = 1,
        NEAR_FIELDED = 2,
        FIELDED = 3
    }
}
