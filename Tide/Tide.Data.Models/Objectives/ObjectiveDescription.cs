using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveDescription
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ObjectiveId { get; set; }
        public ObjectiveCycle? Objective { get; set; }
    }
}
