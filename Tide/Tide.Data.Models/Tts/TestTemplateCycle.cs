using Tide.Data.Models.Objectives;
using Tide.Data.Models.Tcs;

namespace Tide.Data.Models.Tts
{
    public class TestTemplateCycle
    {
        public TestTemplateCycle()
        {
            Objectives = new List<ObjectiveTtMap>();
            Duplicates = new List<TestTemplateCycle>();
            Tests = new List<TestCase>();
        }
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int Year { get; set; }
        public int TestTemplateId { get; set; }
        public int? DiffusionId { get; set; }
        public decimal Similarity { get; set; }
        public decimal DiffusionSimilarity { get; set; }
        public int TestsCount { get; set; }
        public int Anomaly { get; set; }

        public TestTemplate? Template { get; set; }
        public TestTemplateCycle? Diffusion { get; set; }
        public ICollection<ObjectiveTtMap> Objectives { get; set; }
        public ICollection<TestTemplateCycle> Duplicates { get; set; }
        public ICollection<TestCase> Tests { get; set; }
    }
}
