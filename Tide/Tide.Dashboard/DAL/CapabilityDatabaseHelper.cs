using Microsoft.EntityFrameworkCore;
using Tide.Data.Ef;
using Tide.Data.Models.Capabilities;
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

        public List<CapabilityInteroperability> GetInteroperability(int capabilityId, int startCycle, int cyclesCount)
        {
            var relatedCycles = _context.Capabilities.Include(cc => cc.Cycles)
                                     .FirstOrDefault(cc=>cc.Id == capabilityId)
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
