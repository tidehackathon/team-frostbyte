using System;
using Microsoft.AspNetCore.Mvc;

using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using static Tide.Dashboard.DAL.MultiDomainDatabaseHelper;

namespace Tide.Dashboard.Controllers
{
    public class MultiDomainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

  

        public IActionResult TestingTimelineFocusArea()
        {
            int cyclesCount = Utils.CyclesCount;
            int startCycle = Utils.StartCycle;
            var convertor = new TimelineAxesModelConvertor<TimelineMultyDomainModel>();
            var helper = new MultiDomainDatabaseHelper();
            List<string> focusAreaNames = helper.GetAllFocusAreaName();
            List<int> years = new List<int>();
            for (int year = startCycle; year < startCycle + cyclesCount; year++)
                years.Add(year);

            foreach (var x in focusAreaNames)
            {
                convertor.AddGroup(
                    data: helper.GetValuesTimeline(x, years: years),
                    Name: x,
                    mappingFunction: to => new TimelineAxesModel.Object() { Value = to.Value, Year = to.Year });
            }

            convertor.SetEndAndStart(yearStart: startCycle, yearEnd: startCycle + cyclesCount);

            return Json(convertor.Convert());
        }
    }
}

