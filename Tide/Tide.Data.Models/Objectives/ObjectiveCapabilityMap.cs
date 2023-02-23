using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveCapabilityMap
    {
        public int Id { get; set; }
        public int CapabilityId { get; set; }
        public int ObjectiveId { get; set; }
        public decimal InteroperabilityScore { get; set; }
        public ObjectiveCycle? Objective { get; set; }
        public CapabilityCycle? Capability { get; set; }
    }
}
