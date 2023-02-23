using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Capabilities
{
    public class CapabilityDescription
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public int CapabilityId;
        public CapabilityCycle? Capability { get; set; }
    }
}
