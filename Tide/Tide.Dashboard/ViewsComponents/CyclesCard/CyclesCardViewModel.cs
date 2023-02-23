namespace Tide.Dashboard.ViewsComponents.CyclesCard
{
    public class CyclesCardViewModel
    {

        public string Name { get; set; } = null!;

        public List<(int Cycle, string View)> ViewsData { get; set; } = null!;
    }
}
