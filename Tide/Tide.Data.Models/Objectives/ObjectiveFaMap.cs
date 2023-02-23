using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;
using Tide.Data.Models.Standards;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveFaMap
    {
        public int Id { get; set; }
        public int FaId { get; set; }
        public int ObjectiveId { get; set; }

        public ObjectiveCycle? Objective { get; set; }
        public FocusAreaCycle? FocusArea { get; set; }
    }
}
