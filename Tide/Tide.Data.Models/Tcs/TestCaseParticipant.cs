using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;

namespace Tide.Data.Models.Tcs
{
    public class TestCaseParticipant
    {
        public int Id { get; set; }
        public Participant Type { get; set; }
        public TestCaseResult Result { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public int TestId { get; set; }
        public int CapabilityId { get; set; }
        public int Value { get; set; }
        public TestCase? Test { get; set; }
        public CapabilityCycle? Capability { get; set; }
    }
}
