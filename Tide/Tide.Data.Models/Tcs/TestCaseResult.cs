using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Tcs
{
    public enum TestCaseResult : int
    {
        SUCCESS = 0,
        LIMITED_SUCCESS = 1,
        INTEROPERABILITY_ISSUE = 2,
        NOT_TESTED = 3
    }
}
