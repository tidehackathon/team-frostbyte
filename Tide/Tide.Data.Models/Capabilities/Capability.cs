using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Capabilities
{
    public class Capability
    {
        public Capability()
        {
            Cycles = new List<CapabilityCycle>();
        }
        public int Id { get; set; }
        public int NationId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Nation? Nation { get; set; }
        public ICollection<CapabilityCycle> Cycles { get; set; }
    }
}
