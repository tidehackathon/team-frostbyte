using Microsoft.EntityFrameworkCore;
using Tide.Data.Ef;
using Tide.Data.Models;

namespace Tide.Dashboard.DAL
{
    public class NationDatabaseHelper
    {
        private readonly TideContext _context;

        public NationDatabaseHelper()
        {
            _context = Context.Db;
        }


        public record NationInteroperability(int Year, decimal Interoperability);


        public (List<NationInteroperability> BaseInteroperability, List<NationInteroperability> CurrentInteroperability) GetPartialInteroperability(int nationId, int startCycle, int cyclesCount)
        {
            var nation = _context.Nations.First(x => x.Id == nationId);
            var caps = _context.CapabilityCicles.Include(x => x.Capability).Where(x => x.Capability.NationId == nationId);

            List<NationInteroperability> baseResult = new();
            List<NationInteroperability> currentResult = new();

            foreach (var yr in Context.Folders)
            {
                int year = 2000 + yr;
                int count = caps.Where(x => x.Year == year).Count();
                decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
                decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

                bi = count == 0 ? 0 : bi / count;
                ci = count == 0 ? 0 : ci / count;

                baseResult.Add(new NationInteroperability(year, bi));
                currentResult.Add(new NationInteroperability(year, ci));
            }

            baseResult = baseResult.OrderBy(x => x.Year).ToList();
            currentResult = currentResult.OrderBy(x => x.Year).ToList();

            // Add empty cycles if required
            for (int year = startCycle; year < startCycle + cyclesCount; year++)
            {
                if (!baseResult.Any(value => value.Year == year))
                {
                    baseResult.Add(new NationInteroperability(year, baseResult.FirstOrDefault(item => item.Year == year - 1)?.Interoperability ?? 0m));
                }
                if (!currentResult.Any(value => value.Year == year))
                {
                    currentResult.Add(new NationInteroperability(year, currentResult.FirstOrDefault(item => item.Year == year - 1)?.Interoperability ?? 0m));
                }
            }

            return (baseResult, currentResult);
        }

        public List<NationInteroperability> GetInteroperability(int nationId, int startCycle, int cyclesCount)
        {
            var nation = _context.Nations.First(x => x.Id == nationId);
            var caps = _context.CapabilityCicles.Include(x => x.Capability).Where(x => x.Capability.NationId == nationId);

            List<NationInteroperability> result = new();

            foreach (var yr in Context.Folders)
            {
                int year = 2000 + yr;
                int count = caps.Where(x => x.Year == year).Count();
                decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
                decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

                bi = count == 0 ? 0 : bi / count;
                ci = count == 0 ? 0 : ci / count;

                result.Add(new NationInteroperability(year, (bi + ci) / 2));
            }

            result = result.OrderBy(x => x.Year).ToList();

            // Add empty cycles if required
            for (int year = startCycle; year < startCycle + cyclesCount; year++)
            {
                if (!result.Any(value => value.Year == year))
                {
                    result.Add(new NationInteroperability(year, result.FirstOrDefault(item => item.Year == year - 1)?.Interoperability ?? 0m));
                }
            }

            return result;
        }

    }

}
