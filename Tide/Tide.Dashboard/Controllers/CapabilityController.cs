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
                 color: Utils.GREEN_COLOR,
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

        public IActionResult Network(int capabilityId)
        {

            var convertor = new NeuronalAxesModelConvertor();

            var helper = new CapabilityDatabaseHelper();

            var data = helper.GetCapabilityEvolution(capabilityId);

            HashSet<string> capabilities = new HashSet<string>();

            foreach (var capabilityCycle in data)
            {
                // Add ccnode
                var ccNode = convertor.AddNode(capabilityCycle.Item1.Number, capabilityCycle.Item1.Year.ToString(), 900, capabilityCycle.Item1.Number, capabilities.ToArray());

                // Add standard and focus area nodes to cc node
                var standardsNode = ccNode.AddChild("Standards", "", 300);
                var faNode = ccNode.AddChild("Focus Areas", "", 300);

                foreach (var standard in capabilityCycle.Item2)
                {
                    standardsNode.AddChild(standard.Item1.Name.Substring(0, Math.Min(4, standard.Item1.Name.Length)), standard.Item1.Name, standard.Item2 * 10);
                }

                foreach (var fa in capabilityCycle.Item3)
                {
                    faNode.AddChild(fa.Item1.Name.Substring(0, Math.Min(4, fa.Item1.Name.Length)), fa.Item1.Name, fa.Item2);
                }

                capabilities.Add(capabilityCycle.Item1.Number);
            }

            return Json(convertor.Convert());

        }

        public IActionResult Heatmap(int capabilityId)
        {
            var convertor = new HeatMapAxesModelConvert();
            return null;
        }
    }
}
