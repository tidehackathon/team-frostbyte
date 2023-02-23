namespace Tide.Dashboard.ViewsComponents.Plot
{
    public class PlotViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Height { get; set; } = 300;

        public bool AddCard { get; set; } = true;
    }
}
