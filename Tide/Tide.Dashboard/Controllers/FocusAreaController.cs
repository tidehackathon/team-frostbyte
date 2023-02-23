using Microsoft.AspNetCore.Mvc;
using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using static Tide.Dashboard.DAL.AnomaliesDatabaseHelper;

namespace Tide.Dashboard.Controllers
{
    public class FocusAreaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Evolution()
        {
            var converter = new HorizontalAxesModelConverter<AnomalyModel>();
            var helper = new AnomaliesDatabaseHelper();
            int cyclesCount = Utils.CyclesCount;
            int startCycle = Utils.StartCycle;

            for (int year = startCycle; year < startCycle + cyclesCount; year++)
            {
                converter.AddGroup(
                    data: helper.GetYearAnomalies(year),
                    lineId: year.ToString(),
                    mappingFunction: data => new HorizontalAxesModel.Data()
                    {
                        X = data.Deviation,
                        Y = data.FA
                    });
            }

            return Json(converter.Convert());
        }
    }
}
