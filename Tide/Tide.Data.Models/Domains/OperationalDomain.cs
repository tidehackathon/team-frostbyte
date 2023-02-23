using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Domains
{
    public class OperationalDomain
    {
        public OperationalDomain()
        {
            Capabilities = new List<OperationalDomainCapabilityMap>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<OperationalDomainCapabilityMap> Capabilities { get; set; }
    }
}
