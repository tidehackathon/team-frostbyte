using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Duties
{
    public class Duty
    {
        public Duty()
        {
            Capabilities = new List<DutyCapabilityMap>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<DutyCapabilityMap> Capabilities { get; set; }
    }
}
