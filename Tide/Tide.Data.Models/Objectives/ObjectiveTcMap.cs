using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveTcMap
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int ObjectiveId { get; set; }

        public ObjectiveCycle? Objective { get; set; }
        public TestCase? Test { get; set; }
    }
}
