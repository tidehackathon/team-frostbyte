using Tide.Data.Models.Domains;
using Tide.Data.Models.Duties;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;

namespace Tide.Data.Models.Capabilities
{
    public class CapabilityCycle
    {
        public CapabilityCycle()
        {
            Domains = new List<OperationalDomainCapabilityMap>();
            Duties = new List<DutyCapabilityMap>();
            Standards = new List<StandardCapabilityMap>();
            Fas = new List<CapabilityFaMap>();
            Tests = new List<TestCaseParticipant>();
            Objectives= new List<ObjectiveCapabilityMap>();
        }
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Maturity Maturity { get; set; }
        public WarfareLevel Level { get; set; }
        public int Year { get; set; }
        public int SuccessRate { get; set; }
        public int FailureRate { get; set; }
        public int Count { get; set; }
        public decimal CurrentInteroperability { get; set; }
        public decimal BaseInteroperability { get; set; }
        public int CapabilityId { get; set; }
        public int Power { get; set; }
        public CapabilityDescription? Description { get; set; }

        public ICollection<OperationalDomainCapabilityMap> Domains { get; set; }
        public ICollection<DutyCapabilityMap> Duties { get; set; }
        public ICollection<StandardCapabilityMap> Standards { get; set; }
        public ICollection<CapabilityFaMap> Fas { get; set; }
        public ICollection<TestCaseParticipant> Tests { get; set; }
        public ICollection<ObjectiveCapabilityMap> Objectives { get; set; }
        public Capability? Capability { get; set; }
    }
}
