using Microsoft.AspNetCore.Mvc;

namespace Tide.Dashboard.ViewsComponents.Plot
{
    public class PlotViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string id, string name, int height = 300, bool addCard = true)
        {
            return View("~/ViewsComponents/Plot/Index.cshtml", new PlotViewModel() { Id= id, Name = name, AddCard = addCard, Height = height});
        }
    }
}
