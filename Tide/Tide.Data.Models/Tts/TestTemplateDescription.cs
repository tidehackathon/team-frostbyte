using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Tts
{
    public class TestTemplateDescription
    {
        public int Id { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Preconditions { get; set; } = string.Empty;

        public int TemplateId { get; set; }
        public TestTemplate? Template { get; set; }
    }
}
