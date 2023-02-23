using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;

namespace Tide.Data.Models.Standards
{
    public class StandardCapabilityMap
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int CapabilityId { get; set; }

        public decimal InteroperabilityScore {get;set;}
        public int Count { get; set; }
        public Standard? Standard { get; set; }
        public CapabilityCycle? Capability { get; set; }
    }
}
