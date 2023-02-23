namespace Tide.Dashboard.Models.Anomaly
{
    public class FocusAreaDeviationViewModel
    {
        public class FocusAreaDeviationItem
        {
            public decimal DiffusionSimilarity { get; set; }

            public int Year { get; set; }
        }

        public string FocusAreaName { get; set; } = null!;

        public List<FocusAreaDeviationItem> Items { get; set; } = new List<FocusAreaDeviationItem>();
    }
}
