using Microsoft.AspNetCore.Mvc;
using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using Tide.Dashboard.Models.Nation;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;

namespace Tide.Dashboard.Controllers
{
    using static Tide.Dashboard.DAL.NationDatabaseHelper;
    using NationYearData = Tuple<FocusArea, decimal, decimal>;

    public class NationController : Controller
    {
        public NationController()
        {
        }

        public IActionResult Index(int nationId)
        {
            return View("/Views/Nation/Index.cshtml", new NationViewModel() { NationId = nationId });
        }


        public IActionResult Interoperability(int nationId)
        {
            var converter = new StackedDrawerModelConverter<NationInteroperability>();

            var helper = new NationDatabaseHelper();

            converter.AddGroup(
                data: helper.GetInteroperability(nationId, Utils.StartCycle, Utils.CyclesCount),
                lineId: "Interoperability",
                color: Utils.GREEN_COLOR,
                mappingFunction: item => new StackedDrawerModel.Data
                {
                    X = item.Interoperability,
                    Y = item.Year.ToString()
                });

            return Json(converter.Convert());
        }

        public IActionResult PartialInteroperability(int nationId)
        {
            var converter = new StackedDrawerModelConverter<NationInteroperability>();

            var helper = new NationDatabaseHelper();

            var interoperabilityData = helper.GetPartialInteroperability(nationId, Utils.StartCycle, Utils.CyclesCount);

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


        public IActionResult Evolution(int nationId)
        {
            var converter = new RadarChartModelConverter<NationYearData>();
            var helper = new MultiDomainDatabaseHelper();

            var data = helper.NationalMultiDomain(nationId);

            foreach (var nationCycle in data)
            {
                converter.AddGroup(
                    data: nationCycle.Item2.OrderBy(item => item.Item2).ToList(),
                    lineId: $"CWIX{nationCycle.Item1}",
                    mappingFunction: item =>
                    new RadarChartModel.Data()
                    {
                        Label = item.Item1.Name,
                        Value = item.Item2,
                        Threshold = item.Item3
                    });
            }

            return Json(converter.Convert());
        }
    }
}
