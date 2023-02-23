// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Explorer;

List<Tuple<CapabilityCycle,List<Tuple<FocusArea, decimal,decimal>>>> CapabilityMultiDomain(int capabilityId)
{
    using var db = Context.Db;

    var capability = db.Capabilities.Include(x => x.Cycles).First(x => x.Id == capabilityId);

    var list = new List<Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>>();

    foreach (var cycle in capability.Cycles)
    {
        list.Add(new Tuple<CapabilityCycle, List<Tuple<FocusArea, decimal, decimal>>>(cycle, getYear(cycle.Year, cycle.Id)));
    }

    return list;

    List < Tuple<FocusArea, decimal, decimal>> getYear(int year, int capCycleId)
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


List< List<Tuple<FocusArea, decimal, decimal>>> NatMD(int nation)
{
    using var db = Context.Db;

    var list = new List<List<Tuple<FocusArea, decimal, decimal>>>();
    foreach (var cycle in Context.Folders)
    {
        list.Add(getYear(2000+cycle));
    }

    return list;

    List<Tuple<FocusArea, decimal, decimal>> getYear(int year)
    {
        var fas = db.FocusAreaCycles.Include(x => x.FocusArea).Where(x => x.Year == year);
        var objs = db.ObjectiveCycles.Include(x => x.Fas).Include(x => x.Capabilities.Where(y =>y.Capability.Year==year&& y.Capability.Capability.NationId==nation)).Where(x => x.Year == year && x.Capabilities.Any(y => y.Capability.Capability.NationId==nation)).ToList();

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
    int number=Convert.ToInt32(Math.Floor(dec));
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

var test = NatMD(27);

int i = 0;
i++;