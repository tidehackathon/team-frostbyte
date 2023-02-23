using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;

namespace Tide.Data.Models.Tts
{
    public class TestTemplate
    {
        public TestTemplate()
        {
            Cycles = new List<TestTemplateCycle>();
            Standards = new List<StandardTtMap>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Maturity { get; set; }
        public ICollection<TestTemplateCycle> Cycles { get; set; }
        public ICollection<StandardTtMap> Standards { get; set; }
        public TestTemplateDescription? Description { get; set; }
        public TestTemplateResult? Result { get; set; }

    }
}
