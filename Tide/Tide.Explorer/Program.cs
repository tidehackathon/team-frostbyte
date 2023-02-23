// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tide.Data.Models;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Standards;
using Tide.Explorer;



List<Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>> CapabilityMultiDomain(int capabilityId)
{
    using var db = Context.Db;

    var capability = db.Capabilities.Include(x => x.Cycles).First(x => x.Id == capabilityId);

    var list = new List<Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>>();

    foreach (var cycle in capability.Cycles)
    {
        list.Add(new Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>(cycle, getYear(cycle.Year, cycle.Id)));
    }

    return list;

    List<Tuple<FocusArea, decimal, decimal>> getYear(int year, int capCycleId)
    {
        var fas = db.FocusAreaCycles.Include(x => x.FocusArea).Where(x => x.Year == year);
        var objs = db.ObjectiveCycles.Include(x => x.Fas).Include(x => x.Capabilities.Where(y => y.CapabilityId == capCycleId)).Where(x => x.Year == year && x.Capabilities.Any(y => y.CapabilityId == capCycleId)).ToList();

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
            scores.Add(new Tuple<FocusArea, decimal, decimal>(fa.FocusArea, score, score == 0 ? 10 : RoundUp(score)));
        }
        return scores;
    }
}


List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>> NatMD(int nation)
{
    using var db = Context.Db;

    var list = new List<Tuple<int, List<Tuple<FocusArea, decimal, decimal>>>>();

    foreach (var cycle in Context.Folders)
    {
        list.Add(new(cycle, getYear(2000 + cycle)));
    }

    return list;

    List<Tuple<FocusArea, decimal, decimal>> getYear(int year)
    {
        var fas = db.FocusAreaCycles.Include(x => x.FocusArea).Where(x => x.Year == year);
        var objs = db.ObjectiveCycles.Include(x => x.Fas).Include(x => x.Capabilities.Where(y => y.Capability.Year == year && y.Capability.Capability.NationId == nation)).Where(x => x.Year == year && x.Capabilities.Any(y => y.Capability.Capability.NationId == nation)).ToList();

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
            scores.Add(new Tuple<FocusArea, decimal, decimal>(fa.FocusArea, score, score == 0 ? 10 : RoundUp(score)));
        }
        return scores;
    }
}

static decimal RoundUp(decimal dec)
{
    int number = Convert.ToInt32(Math.Floor(dec));
    int remainder = number % 10;
    if (remainder == 0)
    {
        return number;
    }
    else
    {
        return number + (10 - remainder);
    }
}

static decimal MapNumberToRange(decimal num, decimal inputMin, decimal inputMax, decimal outputMin, decimal outputMax)
{
    try
    {
        // Maps a number from one range to another range
        return Convert.ToDecimal(((num - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin));
    }
    catch
    {
        return outputMin;
    }
}

static decimal TenScale(decimal num, decimal inputMin, decimal inputMax) => MapNumberToRange(num, inputMin, inputMax, 0.05M, 10);


static List<Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>> CapEvo(int capId)
{
    using var db = Context.Db;

    var caps = db.CapabilityCicles.Include(x => x.Standards).ThenInclude(x => x.Standard).Include(x => x.Objectives).ThenInclude(x => x.Objective).ThenInclude(x => x.Fas).Where(x => x.CapabilityId == capId);

    var fas = db.FocusAreaCycles.Include(x => x.FocusArea).ToDictionary(x => x.Id, x => x);

    List<Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>> result = new();

    foreach (var cap in caps)
    {
        var standards = cap.Standards.GroupBy(x => x.Standard).Select(x => x.OrderBy(y => y.InteroperabilityScore).Last()).ToList();

        decimal max = standards.Max(x => x.InteroperabilityScore);
        decimal min = standards.Min(x => x.InteroperabilityScore);

        List<Tuple<Standard, decimal>> sresult = standards.Select(x => new Tuple<Standard, decimal>(x.Standard, MapNumberToRange(x.InteroperabilityScore, min, max, 2, 4))).ToList();

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

        List<Tuple<FocusArea, decimal>> fresult = fapairs.Where(x => fas.ContainsKey(x.Key)).Select(x => new Tuple<FocusArea, decimal>(fas[x.Key].FocusArea, MapNumberToRange(x.Value, min, max, 2, 4))).ToList();

        result.Add(new Tuple<CapabilityCycle, List<Tuple<Standard, decimal>>, List<Tuple<FocusArea, decimal>>>(cap, sresult, fresult));
    }

    return result.OrderBy(x => x.Item1.Year).ToList();
}


static List<Tuple<FocusArea, FocusArea, decimal, decimal, decimal>> FaMD(int year)
{
    using var db = Context.Db;


    var fas = db.FocusAreaCycles.Include(x => x.FocusArea)
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
        decimal strength = MapNumberToRange(value.Failure + value.Success, min, max, 5, 100);
        decimal succes = value.Count == 0 ? 0 : (Convert.ToDecimal((Convert.ToDecimal(value.Success) / Convert.ToDecimal(value.Count)) * 100));
        decimal fail = value.Count == 0 ? 0 : (Convert.ToDecimal((Convert.ToDecimal(value.Failure) / Convert.ToDecimal(value.Count)) * 100));

        var item = new Tuple<FocusArea, FocusArea, decimal, decimal, decimal>(fas.First(x => x.Id == value.From).FocusArea, fas.First(x => x.Id == value.To).FocusArea, strength, succes, fail);

        result.Add(item);
    }

    return result;
}

static List<Tuple<Nation, int, decimal, decimal>> NationInterYears(int nationId)
{
    using var db = Context.Db;
    var nation = db.Nations.First(x => x.Id == nationId);
    var caps = db.CapabilityCicles.Include(x => x.Capability).Where(x => x.Capability.NationId == nationId);

    List<Tuple<Nation, int, decimal, decimal>> result = new();

    foreach (var yr in Context.Folders)
    {
        int year = 2000 + yr;
        int count = caps.Where(x => x.Year == year).Count();
        decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
        decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

        bi = count == 0 ? 0 : bi / count;
        ci = count == 0 ? 0 : ci / count;

        result.Add(new Tuple<Nation, int, decimal, decimal>(nation, year, bi, ci));
    }

    return result.OrderBy(x => x.Item2).ToList();
}

static List<Tuple<int, decimal, decimal>> CwixInterYears()
{
    using var db = Context.Db;
    var caps = db.CapabilityCicles.Include(x => x.Capability).ToList();

    List<Tuple<int, decimal, decimal>> result = new();

    foreach (var yr in Context.Folders)
    {
        int year = 2000 + yr;
        int count = caps.Where(x => x.Year == year).Count();
        decimal bi = caps.Where(x => x.Year == year).Sum(x => x.BaseInteroperability);
        decimal ci = caps.Where(x => x.Year == year).Sum(x => x.CurrentInteroperability);

        bi = count == 0 ? 0 : bi / count;
        ci = count == 0 ? 0 : ci / count;

        decimal min = bi < ci ? bi : ci;
        decimal max = bi > ci ? bi : ci;

        min-=0.5M;
        max += 0.5M;

        result.Add(new Tuple<int, decimal, decimal>(year, MapNumberToRange(bi,min,max , 1,5), MapNumberToRange(ci, min, max, 1, 5)));
    }

    return result.OrderBy(x => x.Item2).ToList();
}

static List<Tuple<int, List<Tuple<Standard, decimal>>>> HeatCap(int cap)
{
    using var db = Context.Db;

    var caps = db.CapabilityCicles.Include(x => x.Standards).ThenInclude(x => x.Standard).Where(x=>x.CapabilityId==cap).ToList();
   
    var totalStandards = caps.SelectMany(x => x.Standards).GroupBy(x => x.StandardId).Select(x => x.First().Standard).OrderBy(x => x.Id).ToList();

    List<Tuple<int, List<Tuple<Standard, decimal>>>> results = new List<Tuple<int, List<Tuple<Standard, decimal>>>>();

    var grs = caps.GroupBy(x => x.Year).OrderBy(x=>x.Key).ToList();
    Dictionary<int, decimal>? past = null;
    foreach (var gr in grs)
    {
        var standards = gr.SelectMany(x => x.Standards).ToList();
        List<Tuple<Standard, decimal>> list = new();

        List<Tuple<Standard, decimal, int>> items = standards.GroupBy(x => x.StandardId).Select(x => new Tuple<Standard, decimal, int>(x.First().Standard, x.Sum(x => x.InteroperabilityScore), x.Count())).ToList();

        decimal max = items.Max(x => x.Item3 == 0 ? 0 : (x.Item2 / x.Item3));
        decimal min = items.Min(x => x.Item3 == 0 ? 0 : (x.Item2 / x.Item3));

        foreach (var standard in totalStandards)
        {
            var cr = items.FirstOrDefault(x => x.Item1.Id == standard.Id);
            if (cr == null)
            {
                if(past!=null && past.ContainsKey(standard.Id))
                {
                    list.Add(new Tuple<Standard, decimal>(standard, past[standard.Id]));
                }
                else
                {
                    list.Add(new Tuple<Standard, decimal>(standard, 5));
                }
            }
            else
            {
                decimal map = 5 - MapNumberToRange(cr.Item3 == 0 ? 0 : (cr.Item2 / cr.Item3), min, max, 0, 5);
                list.Add(new Tuple<Standard, decimal>(cr.Item1, map));
            }
        }

        past = list.ToDictionary(x => x.Item1.Id, x => x.Item2);
        results.Add(new Tuple<int, List<Tuple<Standard, decimal>>>(gr.Key, list));
    }

    return results;
}

var test = CwixInterYears();

int i = 0;
i++;