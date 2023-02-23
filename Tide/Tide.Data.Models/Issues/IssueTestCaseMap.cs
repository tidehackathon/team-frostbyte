using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Tcs;

namespace Tide.Data.Models.Issues
{
    public class IssueTestCaseMap
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public int TestId { get; set; }

        public IssueCategory? Issue { get; set; }
        public TestCase? Test { get; set; }
    }
}
