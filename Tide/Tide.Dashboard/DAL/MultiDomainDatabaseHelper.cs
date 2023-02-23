using System;
using Microsoft.EntityFrameworkCore;
using Tide.Data.Ef;
using Tide.Dashboard.Converters;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;

namespace Tide.Dashboard.DAL
{
    public class MultiDomainDatabaseHelper
    {
        private readonly TideContext _context;

        public class TimelineMultyDomainModel
        {
            public decimal Value { get; set; }
            public string Year { get; set; } = null!;
        }

        public MultiDomainDatabaseHelper()
        {
            _context = Context.Db;
        }

        public List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>> NationalMultiDomain(int nation)
        {
            var list = new List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>>();
            
            foreach (var cycle in Context.Folders)
            {
                list.Add(new (cycle, getYear(cycle)));
            }

            return list;

            List<Tuple<FocusArea, decimal, decimal>> getYear(int year)
            {
                var fas = _context.FocusAreaCycles.Include(x => x.FocusArea).Where(x => x.Year == year);
                var objs = _context.ObjectiveCycles.Include(x => x.Fas).Include(x => x.Capabilities.Where(y => y.Capability.Year == year && y.Capability.Capability.NationId == nation)).Where(x => x.Year == year && x.Capabilities.Any(y => y.Capability.Capability.NationId == nation)).ToList();

                List<Tuple<FocusArea, decimal, decimal>> scores = new();

                foreach (var fa in fas)
                {
                    decimal score = 0;
                    var fojs = objs.Where(x => x.Fas.Any(y => y.FaId == fa.Id)).ToList();
                    if (fojs.Count > 0)
                    {
                        int count = fojs.Sum(x => x.Capabilities.Count) + 1;
                        score = fojs.Sum(x => x.Capabilities.Sum(z => z.InteroperabilityScore)) + 10;
                        if (count > 0)
                            score = score / count;


                    }
                    scores.Add(new Tuple<FocusArea, decimal, decimal>(fa.FocusArea, score, score == 0 ? 10 : Utils.RoundUp(score)));
                }
                return scores;
            }
        }


        public List<Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>> CapabilityMultiDomain(int capabilityId)
        {
            var capability = _context.Capabilities.Include(x => x.Cycles).First(x => x.Id == capabilityId);

            var list = new List<Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>>();

            foreach (var cycle in capability.Cycles)
            {
                list.Add(new Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>(cycle, getYear(cycle.Year, cycle.Id)));
            }

            return list;

            List<Tuple<FocusArea, decimal, decimal>> getYear(int year, int capCycleId)
            {
                var fas = _context.FocusAreaCycles.Include(x => x.FocusArea).Where(x => x.Year == year);
                var objs = _context.ObjectiveCycles.Include(x => x.Fas).Include(x => x.Capabilities.Where(y => y.CapabilityId == capCycleId)).Where(x => x.Year == year && x.Capabilities.Any(y => y.CapabilityId == capCycleId)).ToList();

                List<Tuple<FocusArea, decimal, decimal>> scores = new();

                foreach (var fa in fas)
                {
                    decimal score = 0;
                    var fojs = objs.Where(x => x.Fas.Any(y => y.FaId == fa.Id)).ToList();
                    if (fojs.Count > 0)
                    {
                        int count = fojs.Sum(x => x.Capabilities.Count) + 1;
                        score = fojs.Sum(x => x.Capabilities.Sum(z => z.InteroperabilityScore)) + 10;
                        if (count > 0)
                            score = score / count;


                    }
                    scores.Add(new Tuple<FocusArea, decimal, decimal>(fa.FocusArea, score, score == 0 ? 10 : Utils.RoundUp(score)));
                }
                return scores;
            }
        }


        public List<TimelineMultyDomainModel> GetValuesTimeline(string name, List<int> years)
        {
            var values = _context.FocusAreaCycles.Include(t => t.FocusArea).Where(t => t.FocusArea!.Name == name).ToList();
            List<TimelineMultyDomainModel> vals = new List<TimelineMultyDomainModel>();
            foreach (var val in values)
            {
                vals.Add(new TimelineMultyDomainModel() { Value = val.Interoperability, Year = val.Year.ToString() });
            }
            foreach (var year in years)
            {
                bool exists = false;
                foreach (var val in vals)
                {
                    if (int.Parse(val.Year) == year)
                        exists = true;
                }
                if (exists == false)
                    vals.Add(new TimelineMultyDomainModel() { Value = 0, Year = year.ToString() });

            }
            vals.Sort((x, y) => x.Year.CompareTo(y.Year));

            return vals;
        }
        public List<string> GetAllFocusAreaName()
        {
            List<string> names;
            var focusAreas = _context.FocusAreas.ToList();
            names = focusAreas.Select(x => x.Name).ToList();
            return names;
        }

    }
}

