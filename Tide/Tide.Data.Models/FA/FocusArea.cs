using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.FA
{
    public class FocusArea
    {
        public FocusArea()
        {
            Cicles = new List<FocusAreaCycle>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<FocusAreaCycle> Cicles { get; set; }
    }
}
