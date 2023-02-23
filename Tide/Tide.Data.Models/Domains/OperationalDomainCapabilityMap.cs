

using Tide.Data.Models.Capabilities;

namespace Tide.Data.Models.Domains
{
    public class OperationalDomainCapabilityMap
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public int CapabilityId { get; set; }

        public OperationalDomain? Domain { get; set; }
        public CapabilityCycle? Capability { get; set; }
    }
}
