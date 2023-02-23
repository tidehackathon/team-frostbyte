using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Standards
{
    public class Standard
    {
        public Standard()
        {
            Capabilities = new List<StandardCapabilityMap>();
            Objectives = new List<StandardObjectiveMap>();
            Templates = new List<StandardTtMap>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public ICollection<StandardCapabilityMap> Capabilities { get; set; }
        public ICollection<StandardObjectiveMap> Objectives { get; set; }
        public ICollection<StandardTtMap> Templates { get; set; }
    }
}
