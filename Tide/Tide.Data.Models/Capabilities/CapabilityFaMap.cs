using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;
using Tide.Data.Models.Objectives;

namespace Tide.Data.Models.Capabilities
{
    public class CapabilityFaMap
    {
        public int Id { get; set; }
        public int FaId { get; set; }
        public int CapabilityId { get; set; }

        public CapabilityCycle? Capability { get; set; }
        public FocusAreaCycle? FocusArea { get; set; }
    }
}
