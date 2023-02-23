using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tide.Data.Ef;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Standards;
using static Tide.Dashboard.DAL.AnomaliesDatabaseHelper;

namespace Tide.Dashboard.DAL
{
    public class CapabilityDatabaseHelper
    {
        private readonly TideContext _context;

        public CapabilityDatabaseHelper()
        {
            _context = Context.Db;
        }

        public Capability GetCapability(int capabilityId) => _context.Capabilities.AsNoTracking().First(cc => cc.Id == capabilityId);

        public List<Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>> GetCapabilityEvolution(int capabilityId)
        {
            var caps = _context.CapabilityCicles.Include(x => x.Standards).ThenInclude(x => x.Standard).Include(x => x.Objectives).ThenInclude(x => x.Objective).ThenInclude(x => x.Fas).Where(x => x.CapabilityId == capabilityId);

            var fas = _context.FocusAreaCycles.Include(x => x.FocusArea).ToDictionary(x => x.Id, x => x);

            List<Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>> result = new();

            foreach (var cap in caps)
            {
                var standards = cap.Standards.GroupBy(x => x.Standard).Select(x => x.OrderBy(y => y.InteroperabilityScore).Last()).ToList();

                decimal max = standards.Count == 0 ? 0 : standards.Max(x => x.InteroperabilityScore);
                decimal min = standards.Count == 0 ? 0 : standards.Min(x => x.InteroperabilityScore);

                List<Tuple<Standard, decimal>> sresult = standards.Select(x => new Tuple<Standard, decimal>(x.Standard, Utils.MapNumberToRange(x.InteroperabilityScore, min, max, 2, 4))).ToList();

                Dictionary<int, decimal> fapairs = new();
                foreach (var obj in cap.Objectives)
                {
                    foreach (var fa in obj.Objective.Fas)
                    {
                        if (fapairs.ContainsKey(fa.FaId))
                            fapairs[fa.FaId] = (fapairs[fa.FaId] + obj.InteroperabilityScore) / 2;
                        else
                            fapairs[fa.FaId] = obj.InteroperabilityScore;
                    }
                }

                max = fapairs.Max(x => x.Value);
                min = fapairs.Min(x => x.Value);

                List<Tuple<FocusArea, decimal>> fresult = fapairs.Where(x => fas.ContainsKey(x.Key)).Select(x => new Tuple<FocusArea, decimal>(fas[x.Key].FocusArea, Utils.MapNumberToRange(x.Value, min, max, 2, 4))).ToList();

                result.Add(new Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>(cap, sresult, fresult));
            }

            return result.OrderBy(x => x.Item1.Year).ToList();
        }

        public List<CapabilityInteroperability> GetInteroperability(int capabilityId, int startCycle, int cyclesCount)
        {
            var relatedCycles = _context.Capabilities.Include(cc => cc.Cycles)
                                     .FirstOrDefault(cc => cc.Id == capabilityId)
                                    !.Cycles
                                     .ToList();

            var result = relatedCycles.Select(c => new CapabilityInteroperability(c.Year, c.BaseInteroperability + c.CurrentInteroperability))
                                       .ToList();

            // Add empty cycles if required
            for (int year = startCycle; year < startCycle + cyclesCount; year++)
            {
                if (!result.Any(value => value.Year == year))
                {
                    result.Add(new CapabilityInteroperability(year, result.FirstOrDefault(item => item.Year == year - 1)?.Interoperability ?? 0m));
                }
            }

            return result.OrderBy(item => item.Year).ToList();
        }

        public (List<CapabilityInteroperability> BaseInteroperability, List<CapabilityInteroperability> CurrentInteroperability) GetPartialInteroperability(int capabilityId, int startCycle, int cyclesCount)
        {
            var relatedCycles = _context.Capabilities.Include(cc => cc.Cycles)
                                .FirstOrDefault(cc => cc.Id == capabilityId)
                               !.Cycles
                                .ToList();

            var result = relatedCycles.Select(c => BuildResponseItem(c.CurrentInteroperability, c.BaseInteroperability, c.Year))
                                       .ToList();

            // Add empty cycles if required
            for (int year = startCycle; year < startCycle + cyclesCount; year++)
            {
                if (!result.Any(value => value.CurrentInteroperability.Year == year))
                {
                    var previousItem = result.FirstOrDefault(item => item.CurrentInteroperability.Year == year - 1);

                    var defaultItem = previousItem == default ?
                        BuildResponseItem(0m, 0m, year) :
                        BuildResponseItem(previousItem.CurrentInteroperability.Interoperability, previousItem.BaseInteroperability.Interoperability, year);

                    result.Add(defaultItem);
                }
            }

            result = result.OrderBy(item => item.CurrentInteroperability.Year).ToList();

            var bi = result.Select(item => item.BaseInteroperability).ToList();
            var ci = result.Select(item => item.CurrentInteroperability).ToList();

            return (bi, ci);

            (CapabilityInteroperability BaseInteroperability, CapabilityInteroperability CurrentInteroperability) BuildResponseItem(decimal currentInteroperability, decimal baseInteroperability, int year)
                => (BaseInteroperability: new CapabilityInteroperability(year, baseInteroperability),
                    CurrentInteroperability: new CapabilityInteroperability(year, currentInteroperability));
        }
    }

    public record CapabilityInteroperability(int Year, decimal Interoperability);
}
