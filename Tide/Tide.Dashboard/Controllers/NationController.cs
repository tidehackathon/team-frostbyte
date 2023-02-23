using Microsoft.AspNetCore.Mvc;
using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using Tide.Dashboard.Models.Nation;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;

namespace Tide.Dashboard.Controllers
{
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
