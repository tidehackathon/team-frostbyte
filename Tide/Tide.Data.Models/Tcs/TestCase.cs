using Tide.Data.Models.Issues;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Tcs
{
    public class TestCase
    {
        public TestCase()
        {
            Participants = new List<TestCaseParticipant>();
            Issues = new List<IssueTestCaseMap>();
            Objectives = new List<ObjectiveTcMap>();
        }
        public int Id { get; set; }
        public int? TemplateId { get; set; }

        public string Number { get; set; } = string.Empty;
        public int Year { get; set; }
        public TestCaseResult Result { get; set; }
        public bool Shortfall { get; set; }
        public int ParticipantsCount { get; set; }
        
        

        public TestTemplateCycle? Template { get; set; }
        public ICollection<TestCaseParticipant> Participants { get; set; }
        public ICollection<IssueTestCaseMap> Issues { get; set; }
        public ICollection<ObjectiveTcMap> Objectives { get; set; }

    }
}
