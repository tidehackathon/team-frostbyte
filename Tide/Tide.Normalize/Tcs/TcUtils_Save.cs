using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Tcs;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Tts;

namespace Tide.Normalize.Tcs
{
    internal static partial class TcUtils
    {
        private static readonly TestCaseResult DEFAULT_TESTCASE_RESULT = TestCaseResult.NOT_TESTED;

        public static void Save()
        {
            List<TestCase> dbTestCases = new List<TestCase>();

            using var context = Context.Db;

            foreach (var folder in Context.Folders)
            {
                var models = Tcs(folder);

                dbTestCases.AddRange(models.Select(ConvertToTestCase));
            }
            dbTestCases = dbTestCases.Where(x => !x.Participants.Any(y => y.CapabilityId == 0)).ToList();

            int csize = 200;
            int limit = dbTestCases.Count / csize + (dbTestCases.Count % csize == 0 ? 0 : 1);
            for (int i = 0; i < limit; i++)
            {
                try
                {
                    var chunk = dbTestCases.Skip(i * csize).Take(csize).ToList();
                    context.Tests.AddRange(chunk);
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    int x = 0;
                    x++;
                }
            }

            TestCaseParticipant ConvertToTestCaseParticipant(ParticipantResult participantResult, Model model, TestCase testCase, Participant type)
             => new TestCaseParticipant()
             {
                 CapabilityId = CapabilitiesUtils.Get(participantResult.Id, model.Year, context)?.Id ?? 0,
                 Remarks = participantResult.Remarks,
                 Result = ConvertTestCaseResultFromString(participantResult.Status) ?? DEFAULT_TESTCASE_RESULT,
                 Test = testCase,
                 Type = type
             };


            TestCase ConvertToTestCase(Model model)
            {
                var result = new TestCase()
                {
                    Number = model.Number,
                    Year = model.Year,
                    Shortfall = model.FinalResult!.ShortfallInvestigation,
                    Result = ConvertTestCaseResultFromString(model.FinalResult.Result) ?? DEFAULT_TESTCASE_RESULT,
                    Participants = new List<TestCaseParticipant>(),
                    TemplateId = TtUtils.Get(model.TestTemplateNumber, model.Year, context)?.Id
                };

                if (model.PartialResult?.Providers.Length > 0)
                {
                    foreach (var provider in model.PartialResult.Providers)
                    {
                        result.Participants.Add(ConvertToTestCaseParticipant(provider, model, result, Participant.PROVIDER));
                    }
                }

                if (model.PartialResult?.Consumers.Length > 0)
                {
                    foreach (var provider in model.PartialResult.Consumers)
                    {
                        result.Participants.Add(ConvertToTestCaseParticipant(provider, model, result, Participant.CONSUMER));
                    }
                }


                return result;
            }
        }

        public static TestCaseResult? ConvertTestCaseResultFromString(string testCaseResultString) => testCaseResultString switch
        {
            "Success" => TestCaseResult.SUCCESS,
            "Interoperability Issue" => TestCaseResult.INTEROPERABILITY_ISSUE,
            "Limited Success" => TestCaseResult.LIMITED_SUCCESS,
            "Not Tested" => TestCaseResult.NOT_TESTED,
            _ => null
        };

    }
}
