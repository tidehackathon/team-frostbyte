using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Data.Models.Tts
{
    public class TestTemplateResult
    {
        public int Id { get; set; }
        public string Success { get; set; } = string.Empty;
        public string Limited { get; set; } = string.Empty;
        public string Interoperability { get; set; } = string.Empty;
        public int TemplateId { get; set; }

        public TestTemplate? Template { get; set; }
    }
}
