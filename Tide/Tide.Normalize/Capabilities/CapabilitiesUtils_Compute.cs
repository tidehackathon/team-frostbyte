using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;

namespace Tide.Normalize.Capabilities
{
    internal static partial class CapabilitiesUtils
    {
        public static void ComputeSuccessRate()
        {
            using var db = Context.Db;

            int count = db.Tests.Count();
            int batch = 200;
            int limit = count / batch + (count % batch > 0 ? 1 : 0);

            for (int i = 0; i < limit; i++)
            {
                IQueryable<TestCase> query = db.Tests.Include(x => x.Participants).ThenInclude(x => x.Capability);
                query = query.Include(x => x.Template);
                var tcs = query.OrderBy(x => x.Id).Skip(batch * i).Take(batch).ToList();
                foreach (var tc in tcs)
                {
                    if (tc.Participants.Count < 2) continue;
                    foreach (var participant in tc.Participants)
                    {
                        if (participant.Capability == null)
                            continue;
                        if (participant.Result == TestCaseResult.SUCCESS || participant.Result == TestCaseResult.LIMITED_SUCCESS)
                            participant.Capability.SuccessRate++;
                        if (participant.Result == TestCaseResult.INTEROPERABILITY_ISSUE)
                            participant.Capability.FailureRate++;
                        participant.Capability.Count++;
                        participant.Value = TestCaseResultConvert(participant.Result);
                    }
                }
                db.SaveChanges();
                Console.WriteLine($"{i} from {limit}");
            }

        }

        public static void ComputeStandardSuccessRate()
        {
            using var db = Context.Db;

            int count = db.Templates.Count();
            int batch = 200;
            int limit = count / batch + (count % batch > 0 ? 1 : 0);

            for (int i = 0; i < limit; i++)
            {
                IQueryable<TestTemplateCycle> query = db.TemplateCycles.Include(x => x.Template).ThenInclude(x => x.Standards).Include(x => x.Diffusion).ThenInclude(x => x.Template);

                query = query.Include(x => x.Tests).ThenInclude(x => x.Objectives).ThenInclude(x => x.Objective).ThenInclude(x => x.Standards);
                query = query.Include(x => x.Tests).ThenInclude(x => x.Participants).ThenInclude(x => x.Capability).ThenInclude(x => x.Standards);
                var tts = query.OrderBy(x => x.Id).Skip(batch * i).Take(batch).ToList();
                foreach (var tt in tts)
                {
                    if (tt.Tests.Count == 0)
                        continue;
                    int maturity = tt.Diffusion?.Template?.Maturity ?? tt.Template?.Maturity ?? 1;

                    foreach (var tc in tt.Tests)
                    {
                        var standards = new HashSet<int>(tc.Objectives.SelectMany(x => x.Objective.Standards.Select(x => x.StandardId)).ToList() ?? new List<int>());
                        standards.Intersect(tt.Template.Standards.Select(x => x.StandardId));
                        if (tc.Participants.Count == 0)
                            continue;
                        foreach (var participant in tc.Participants)
                        {
                            foreach (var standard in participant.Capability.Standards)
                                if (standards.Contains(standard.StandardId))
                                {
                                    standard.Count++;
                                    standard.InteroperabilityScore += Convert.ToDecimal(participant.Value * (1 + maturity * 0.02));
                                }
                        }

                    }
                }
                db.SaveChanges();
                Console.WriteLine($"{i} from {limit}");
            }
        }

        public static void CalculateCapabilityPower()
        {
            using var db = Context.Db;

            var capabilities = db.Capabilities.Include(x => x.Cycles.OrderBy(x => x.Year)).ToList();
            foreach (var capability in capabilities)
            {
                for (int i = 0; i < capability.Cycles.Count; i++)
                    capability.Cycles.ElementAt(i).Power = i + 1;
            }
            db.SaveChanges();
        }

        public static void CalculateCapabilityInteroperability()
        {
            using var db = Context.Db;

            var capabilities = db.Capabilities.Include(x => x.Cycles.OrderBy(x => x.Year)).ThenInclude(x => x.Standards).ToList();
            var cicles = capabilities.SelectMany(x => x.Cycles).OrderBy(x => x.CapabilityId).ThenBy(x => x.Year).ToList();

            foreach (var cycle in cicles)
            {
                computeInteroperability(cycle);
            }

            db.SaveChanges();

            void computeInteroperability(CapabilityCycle cycle)
            {
                var past = capabilities.First(x => x.Id == cycle.CapabilityId).Cycles.FirstOrDefault(x => x.Year < cycle.Year);
                var pastStandards = past?.Standards?.Select(x => x.StandardId)?.ToHashSet() ?? new HashSet<int>();

                decimal exponent = Convert.ToDecimal(1 + cycle.Power * 0.1 + ((int)cycle.Maturity) * 0.05);

                cycle.CurrentInteroperability = cycle.Standards.Where(x => !pastStandards.Contains(x.Id)).Sum(x => exponent * (x.Count == 0 ? 0 : x.InteroperabilityScore / x.Count));

                if (pastStandards.Count == 0 || past == null)
                    cycle.BaseInteroperability = cycle.CurrentInteroperability;
                else
                {
                    var cstandards = cycle.Standards.Where(x => pastStandards.Contains(x.Id) && x.InteroperabilityScore > 0).Select(x => x.StandardId).ToHashSet();
                    cycle.BaseInteroperability = cycle.Standards.Where(x => cstandards.Contains(x.Id)).Sum(x => exponent * (x.Count == 0 ? 0 : x.InteroperabilityScore / x.Count));
                    var nottested = pastStandards.Except(cstandards).ToHashSet();
                    if (nottested.Count() > 0)
                    {
                        cycle.BaseInteroperability += past.Standards.Where(x => nottested.Contains(x.Id)).Sum(x => exponent * (x.Count == 0 ? 0 : x.InteroperabilityScore / x.Count));
                        if (cycle.BaseInteroperability == 0)
                            cycle.BaseInteroperability = past.BaseInteroperability;
                    }
                    if (cycle.BaseInteroperability == 0)
                        cycle.BaseInteroperability = cycle.CurrentInteroperability;
                }
            }
        }

        public static void CalculateFaInteroperability()
        {
            using var db = Context.Db;
            var fas = db.FocusAreaCycles.Include(x => x.Capabilities).ThenInclude(x => x.Capability).ToList();
            foreach (var fa in fas)
            {
                if (fa.Capabilities.Count > 0)
                    fa.Interoperability = fa.Capabilities.Sum(x => x.Capability.BaseInteroperability + x.Capability.CurrentInteroperability) / fa.Capabilities.Count;
            }

            db.SaveChanges();
        }

        public static void CalculateCapabilityObjectiveInteroperability()
        {
            using var db = Context.Db;

            IQueryable<CapabilityCycle> cicles = db.CapabilityCicles.Include(x => x.Objectives).ThenInclude(x => x.Objective).ThenInclude(x => x.Standards);
            cicles = cicles.Include(x => x.Standards);

            foreach (var cicle in cicles)
            {
                Dictionary<int,StandardCapabilityMap> standards=cicle.Standards.GroupBy(x=>x.StandardId).ToDictionary(x=>x.Key,x=>x.OrderBy(y=>y.InteroperabilityScore).First());
                foreach (var obj in cicle.Objectives)
                {
                    int count = obj.Objective.Standards.Where(y=> standards.ContainsKey(y.StandardId) && standards[y.StandardId].Count>0).Count();
                    decimal sum = obj.Objective.Standards.Where(y => standards.ContainsKey(y.StandardId) && standards[y.StandardId].Count > 0).Sum(y => standards[y.StandardId].InteroperabilityScore/ standards[y.StandardId].Count);
                    if (count > 0)
                        obj.InteroperabilityScore = sum / count;
                }
            }

            db.SaveChanges();
        }

        public static void CalculateObjectiveInteroperability()
        {
            using var db = Context.Db;

            var objectives = db.ObjectiveCycles.Include(x => x.Capabilities);

            foreach (var objective in objectives)
            {
                if (objective.Capabilities.Count > 0)
                    objective.InteroperabilityScore = objective.Capabilities.Sum(x => x.InteroperabilityScore) / objective.Capabilities.Count;
            }

            db.SaveChanges();
        }

        private static int TestCaseResultConvert(TestCaseResult result)
        {
            switch (result)
            {
                case TestCaseResult.SUCCESS: return 10;
                case TestCaseResult.LIMITED_SUCCESS: return 6;
                case TestCaseResult.INTEROPERABILITY_ISSUE: return -1;
                default: return 0;
            }
        }
    }
}
