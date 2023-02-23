using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Objectives;

namespace Tide.Data.Models.Standards
{
    public class StandardObjectiveMap
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int ObjectiveId { get; set; }

        public Standard? Standard { get; set; }
        public ObjectiveCycle? Objective { get; set; }
    }
}
