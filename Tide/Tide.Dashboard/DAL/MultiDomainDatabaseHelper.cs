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

        public List<MultidomainInteroperability> GetInteroperability()
        {
            var caps = _context.CapabilityCicles.Include(x => x.Capability).ToList();

            List<MultidomainInteroperability> result = new();

            foreach (var yr in Context.Folders)
            {
                int year = 2000 + yr;
                int count = caps.Where(x => x.Year == year).Count();
                decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
                decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

                bi = count == 0 ? 0 : bi / count;
                ci = count == 0 ? 0 : ci / count;

                result.Add(new MultidomainInteroperability(yr, bi + ci));
            }

            return result.OrderBy(x => x.Year).ToList();
        }

        public (List<MultidomainInteroperability> BaseInteroperability, List<MultidomainInteroperability> CurrentInteroperability) GetPartialInteroperability()
        {
            var caps = _context.CapabilityCicles.Include(x => x.Capability).ToList();

            List<MultidomainInteroperability> baseResult = new();
            List<MultidomainInteroperability> currentResult = new();

            foreach (var yr in Context.Folders)
            {
                int year = 2000 + yr;
                int count = caps.Where(x => x.Year == year).Count();
                decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
                decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

                bi = count == 0 ? 0 : bi / count;
                ci = count == 0 ? 0 : ci / count;

                baseResult.Add(new MultidomainInteroperability(yr, ci));
                currentResult.Add(new MultidomainInteroperability(yr, ci));
            }

            return (baseResult, currentResult);
        }

        public List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>> NationalMultiDomain(int nation)
        {
            var list = new List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>>();

            foreach (var cycle in Context.Folders)
            {
                list.Add(new(cycle, getYear(2000 + cycle)));
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
                return scores.OrderBy(item => item.Item2).ToList();
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


        public List<Tuple<FocusArea, FocusArea, decimal, decimal, decimal>> FocusAreaMultiDomain(int year)
        {
            var fas = _context.FocusAreaCycles.Include(x => x.FocusArea)
                .Include(x => x.Objectives)
                .ThenInclude(x => x.Objective)
                .ThenInclude(x => x.Capabilities).Where(x => x.Year == year).ToList();

            List<Tuple<int, int, int, int, int, int>> caps = new();

            Dictionary<Tuple<int, int>, FaMdModel> maps = new();

            foreach (var fa in fas)
            {
                foreach (var obj in fa.Objectives)
                {
                    foreach (var cap in obj.Objective.Capabilities)
                    {
                        Tuple<int, int, int, int, int, int> item = new Tuple<int, int, int, int, int, int>(fa.Id, obj.ObjectiveId, obj.Objective.TcCount, obj.Objective.TcSuccess, obj.Objective.TcFail, cap.CapabilityId);
                        caps.Add(item);
                    }
                }
            }

            var grs = caps.GroupBy(x => x.Item6).ToList();

            foreach (var gr in grs)
            {
                var faces = gr.ToList().OrderBy(x => x.Item1).ToList();

                var fagrs = faces.GroupBy(x => x.Item1).Select(x => new Tuple<int, int, int, int>(x.Key, x.Sum(y => y.Item3), x.Sum(y => y.Item4), x.Sum(y => y.Item5))).ToList();

                for (int i = 0; i < fagrs.Count - 1; i++)
                {
                    for (int j = i + 1; j < fagrs.Count; j++)
                    {
                        if (fagrs[i].Item1 == fagrs[j].Item1)
                            continue;
                        Tuple<int, int> uno = new Tuple<int, int>(fagrs[i].Item1, fagrs[j].Item1);
                        Tuple<int, int> duo = new Tuple<int, int>(fagrs[j].Item1, fagrs[i].Item1);

                        FaMdModel? map = maps.ContainsKey(uno) ? maps[uno] : (maps.ContainsKey(duo) ? maps[duo] : null);

                        if (map == null)
                        {
                            map = new FaMdModel(fagrs[i].Item1, fagrs[j].Item1)
                            {
                                Count = fagrs[i].Item2 + fagrs[j].Item2,
                                Success = fagrs[i].Item3 + fagrs[j].Item3,
                                Failure = fagrs[i].Item4 + fagrs[j].Item4
                            };
                            maps.Add(uno, map);
                        }
                        else
                        {
                            map.Count += fagrs[i].Item2 + fagrs[j].Item2;
                            map.Success += fagrs[i].Item3 + fagrs[j].Item3;
                            map.Failure += fagrs[i].Item4 + fagrs[j].Item4;
                        }

                    }
                }

            }

            List<Tuple<FocusArea, FocusArea, decimal, decimal, decimal>> result = new List<Tuple<FocusArea, FocusArea, decimal, decimal, decimal>>();

            int min = maps.Values.Min(x => x.Failure + x.Success);
            int max = maps.Values.Max(x => x.Failure + x.Success);

            foreach (var value in maps.Values)
            {
                decimal strength = Utils.MapNumberToRange(value.Failure + value.Success, min, max, 5, 100);
                decimal succes = value.Count == 0 ? 0 : (Convert.ToDecimal((Convert.ToDecimal(value.Success) / Convert.ToDecimal(value.Count)) * 100));
                decimal fail = value.Count == 0 ? 0 : (Convert.ToDecimal((Convert.ToDecimal(value.Failure) / Convert.ToDecimal(value.Count)) * 100));

                var item = new Tuple<FocusArea, FocusArea, decimal, decimal, decimal>(fas.First(x => x.Id == value.From).FocusArea, fas.First(x => x.Id == value.To).FocusArea, strength, succes, fail);

                result.Add(item);
            }

            return result;
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

        public record MultidomainInteroperability(int Year, decimal Interoperability);

        internal class FaMdModel
        {
            public FaMdModel(int from, int to)
            {
                From = from;
                To = to;
            }

            public int From { get; private init; }
            public int To { get; private init; }

            public int Count { get; set; }
            public int Success { get; set; }
            public int Failure { get; set; }
        }
    }
}

