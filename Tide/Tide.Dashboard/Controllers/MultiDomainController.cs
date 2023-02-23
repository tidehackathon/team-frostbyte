using System;
using Microsoft.AspNetCore.Mvc;

using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using Tide.Data.Models.FA;
using static Tide.Dashboard.DAL.MultiDomainDatabaseHelper;

namespace Tide.Dashboard.Controllers
{
    public class MultiDomainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FasConnections(int year)
        {
            var converter = new ChordDiagramModelConverter<Tuple<FocusArea, FocusArea, decimal, decimal, decimal>>();

            var helper = new MultiDomainDatabaseHelper();

            var data = helper.FocusAreaMultiDomain(year);

            converter.AddConnections(data,
                mappingFunction: item =>
                {
                    return new ChordDiagramModel.Connection()
                    {
                        From = item.Item1.Name,
                        To = item.Item2.Name,
                        Value = item.Item3,
                        AdditionalText = new List<ChordDiagramModel.AdditionalText>()
                        {
                            new ChordDiagramModel.AdditionalText()
                            {
                                Label = "Test",
                                Value = 5
                            }
                        },
                        ValueLabel = ""
                    };
                });

            return Json(converter.Convert());
        }

        public IActionResult Interoperability()
        {
            var converter = new StackedDrawerModelConverter<MultidomainInteroperability>();

            var helper = new MultiDomainDatabaseHelper();

            converter.AddGroup(
                data: helper.GetInteroperability(),
                lineId: "Interoperability",
                color: Utils.GREEN_COLOR,
                mappingFunction: item => new StackedDrawerModel.Data
                {
                    X = item.Interoperability,
                    Y = item.Year.ToString()
                });

            return Json(converter.Convert());
        }

        public IActionResult PartialInteroperability()
        {
            var converter = new StackedDrawerModelConverter<MultidomainInteroperability>();

            var helper = new MultiDomainDatabaseHelper();

            var interoperabilityData = helper.GetPartialInteroperability();

            converter.AddGroup(
                data: interoperabilityData.BaseInteroperability,
                lineId: "Base Interoperability",
                color: Utils.BLUE_COLOR,
                mappingFunction: item => new StackedDrawerModel.Data
                {
                    X = item.Interoperability,
                    Y = item.Year.ToString()
                });

            converter.AddGroup(
            data: interoperabilityData.CurrentInteroperability,
            lineId: "Current Interoperability",
            color: Utils.YELLOW_COLOR,
            mappingFunction: item => new StackedDrawerModel.Data
            {
                X = item.Interoperability,
                Y = item.Year.ToString()
            });

            return Json(converter.Convert());
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

