using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Objectives
{
    public class Objective
    {
        public Objective()
        {
            Cycles = new List<ObjectiveCycle>();
        }
        public int Id { get; set; }
        public ICollection<ObjectiveCycle> Cycles { get; set; }
    }
}
