using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveCycle
    {
        public ObjectiveCycle()
        {
            Standards = new List<StandardObjectiveMap>();
            Fas = new List<ObjectiveFaMap>();
            Capabilities = new List<ObjectiveCapabilityMap>();
            Templates = new List<ObjectiveTtMap>();
            Tests = new List<ObjectiveTcMap>();
        }
        public int Id { get; set; }
        public int Year { get; set; }
        public int ObjectiveId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal InteroperabilityScore { get; set; }
        public ObjectiveScope Scope { get; set; }
        public ICollection<StandardObjectiveMap> Standards { get; set; }
        public ICollection<ObjectiveFaMap> Fas { get; set; }
        public ICollection<ObjectiveCapabilityMap> Capabilities { get; set; }
        public ICollection<ObjectiveTtMap> Templates { get; set; }
        public ICollection<ObjectiveTcMap> Tests { get; set; }
        public ObjectiveDescription? Description { get; set; }
        public Objective? Objective { get; set; }
    }
}
