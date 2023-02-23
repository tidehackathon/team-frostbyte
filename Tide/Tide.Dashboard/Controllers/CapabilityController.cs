using Microsoft.AspNetCore.Mvc;
using Tide.Dashboard.Converters;
using Tide.Dashboard.DAL;
using Tide.Dashboard.Models.Capability;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;

namespace Tide.Dashboard.Controllers
{
    using CapabilityYearData = Tuple<FocusArea, decimal, decimal>;

    public class CapabilityController : Controller
    {
        public IActionResult Index(int capabilityId)
        {
            var helper = new CapabilityDatabaseHelper();

            var capability = helper.GetCapability(capabilityId);
            
            return View("/Views/Capability/Index.cshtml", new CapabilityViewModel()
            {
                CapabilityName = capability.Name,
                CapabilityId = capabilityId
            });
        }

        public IActionResult Interoperability(int capabilityId)
        {
            var converter = new StackedDrawerModelConverter<CapabilityInteroperability>();

            var helper = new CapabilityDatabaseHelper();

            converter.AddGroup(
                data: helper.GetInteroperability(capabilityId, Utils.StartCycle, Utils.CyclesCount),
                lineId: "Interoperability",
                mappingFunction: item => new StackedDrawerModel.Data
                {
                    X = item.Interoperability,
                    Y = item.Year.ToString()
                });

            return Json(converter.Convert());
        }

        public IActionResult PartialInteroperability(int capabilityId)
        {
            var converter = new StackedDrawerModelConverter<CapabilityInteroperability>();

            var helper = new CapabilityDatabaseHelper();

            var interoperabilityData = helper.GetPartialInteroperability(capabilityId, Utils.StartCycle, Utils.CyclesCount);

            converter.AddGroup(
                data: interoperabilityData.BaseInteroperability,
                lineId: "Base Interoperability",
                mappingFunction: item => new StackedDrawerModel.Data
                {
                    X = item.Interoperability,
                    Y = item.Year.ToString()
                });

            converter.AddGroup(
            data: interoperabilityData.CurrentInteroperability,
            lineId: "Current Interoperability",
            mappingFunction: item => new StackedDrawerModel.Data
            {
                X = item.Interoperability,
                Y = item.Year.ToString()
            });

            return Json(converter.Convert());
        }

        public IActionResult Evolution(int capabilityId)
        {
            var converter = new RadarChartModelConverter<CapabilityYearData>();
            var helper = new MultiDomainDatabaseHelper();

            var data = helper.CapabilityMultiDomain(capabilityId);

            foreach (var capabilityCycle in data)
            {
                converter.AddGroup(
                    data: capabilityCycle.Item2.OrderBy(item => item.Item2).ToList(),
                    lineId: capabilityCycle.Item1.Year.ToString(),
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
