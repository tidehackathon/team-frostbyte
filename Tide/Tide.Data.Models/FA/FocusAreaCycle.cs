using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Objectives;

namespace Tide.Data.Models.FA
{
    public class FocusAreaCycle
    {
        public FocusAreaCycle()
        {
            Objectives=new List<ObjectiveFaMap>();
            Capabilities = new List<CapabilityFaMap>();
        }
        public int Id { get; set; }
        public int Year { get; set; }
        public int FocusAreaId { get; set; }
        public decimal Interoperability { get; set; }
        public ICollection<ObjectiveFaMap> Objectives { get; set; }
        public ICollection<CapabilityFaMap> Capabilities { get; set; }
        public FocusArea? FocusArea { get; set; }
    }
}
