namespace Tide.Dashboard.Models.Anomaly
{
    public class AnomalyViewModel
    {
        public int CyclesCount => Utils.CyclesCount;

        public int StartCycle => Utils.StartCycle;

        public List<FocusAreaDeviationViewModel> FocusAreas { get; set; } = new();
    }
}
