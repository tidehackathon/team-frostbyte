

using Tide.Data.Models.Capabilities;

namespace Tide.Data.Models.Duties
{
    public class DutyCapabilityMap
    {
        public int Id { get; set; }
        public int DutyId { get; set; }
        public int CapabilityId { get; set; }

        public Duty? Duty { get; set; }
        public CapabilityCycle? Capability { get; set; }
    }
}
