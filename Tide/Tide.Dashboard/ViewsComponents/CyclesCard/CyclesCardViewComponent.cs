using Microsoft.AspNetCore.Mvc;

namespace Tide.Dashboard.ViewsComponents.CyclesCard
{
    public class CyclesCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title, List<(int Cycle, string ViewName)> viewsData)
        {
            return View("~/ViewsComponents/CyclesCard/Index.cshtml", new CyclesCardViewModel() { Name = title, ViewsData = viewsData});
        }
    }
}
