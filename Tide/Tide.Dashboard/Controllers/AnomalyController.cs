using Microsoft.AspNetCore.Mvc;
using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using Tide.Dashboard.Models.Anomaly;
using static Tide.Dashboard.DAL.AnomaliesDatabaseHelper;

namespace Tide.Dashboard.Controllers
{
    public class AnomalyController : Controller
    { 
        public IActionResult Index()
        {
            var helper = new AnomaliesDatabaseHelper();

            var focusAreaDiffusions = helper.GetFocusAreaDiffusions(Utils.StartCycle, Utils.CyclesCount);

            var focusAreasModels = focusAreaDiffusions.Select(focusAreaDiffusion => new FocusAreaDeviationViewModel()
            {
                FocusAreaName = focusAreaDiffusion.Name,

                // Order focus area models before return to view
                Items = focusAreaDiffusion.Values.Select(x => new FocusAreaDeviationViewModel.FocusAreaDeviationItem()
                {
                    DiffusionSimilarity = x.DiffusionSimilarity,
                    Year = x.Year
                })
                    .OrderBy(item => item.Year)
                    .ToList()
            }).ToList();


            return View("~/Views/Anomaly/Index.cshtml", new AnomalyViewModel()
            {
                FocusAreas = focusAreasModels
            }); ;
        }

        public IActionResult CyclesDeviation()
        {
            var converter = new StackedDrawerModelConverter<CycleDiffusionModel>();
            var helper = new AnomaliesDatabaseHelper();
            int cyclesCount = Utils.CyclesCount;
            int startCycle = Utils.StartCycle;



            converter.AddGroup(
                data: helper.GetCycleDiffusions(Utils.StartCycle, Utils.CyclesCount),
                lineId: "Difffusion",
                mappingFunction: data => new StackedDrawerModel.Data()
                {
                    X = data.DiffusionSimilarity,
                    Y = data.Name
                });


            return Json(converter.Convert());
        }

        public IActionResult TestingAnomalies()
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
