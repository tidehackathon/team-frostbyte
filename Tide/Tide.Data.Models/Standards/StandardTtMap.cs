using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Standards
{
    public class StandardTtMap
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int TestTemplateId { get; set; }

        public Standard? Standard { get; set; }
        public TestTemplate? TestTemplate { get; set; }
    }
}
