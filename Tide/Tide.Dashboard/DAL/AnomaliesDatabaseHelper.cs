using Microsoft.EntityFrameworkCore;
using Tide.Data.Ef;

namespace Tide.Dashboard.DAL
{
    public class AnomaliesDatabaseHelper
    {
        private readonly TideContext _context;

        public AnomaliesDatabaseHelper()
        {
            _context = Context.Db;
        }

        public class AnomalyModel
        {
            public string FA { get; set; } = null!;

            public decimal Deviation { get; set; }
        }

        public class FADiffusionModel
        {
            public string Name { get; set; } = null!;

            public List<(int Year, decimal DiffusionSimilarity)> Values { get; set; } = null!;
        }

        public class CycleDiffusionModel
        {
            public string Name { get; set; } = null!;

            public decimal DiffusionSimilarity { get; set; }
        }

        public List<AnomalyModel> GetYearAnomalies(int year)
        {
            // Get anomalies for given year.
            var anomalies = _context.TtYearAnomalies.Include(ta => ta.Template)
                                                    .Include(ta => ta.Fa)
                                                    .Where(item => item.Year == year).ToList();

            // Contains fas which have diffusion data.
            HashSet<string> matchedFas = new HashSet<string>();

            // Get anomalies data by grouping by FA ids.
            var anomaliesData = anomalies.GroupBy(item => item.FaId).Select(group =>
            {
                string faName = group.First().Fa!.Name;

                matchedFas.Add(faName);

                return new AnomalyModel()
                {
                    Deviation = (group.Sum(t => t.Template?.DiffusionSimilarity ?? 0m)),
                    FA = faName
                };
            }).ToList();

            // Add fas which are valid and do not contain any diffusion data.
            anomaliesData.AddRange(_context.FocusAreas.Where(fa => !matchedFas.Contains(fa.Name)).ToList().Select(fa => new AnomalyModel()
            {
                FA = fa.Name,
                Deviation = 0
            }));


            return anomaliesData;
        }

        public List<CycleDiffusionModel> GetCycleDiffusions(int startYear, int cyclesCount)
        {
            var cycleData = _context.TtYearAnomalies.Include(ta => ta.Template).ToList().GroupBy(anomaly => anomaly.Year);

            List<CycleDiffusionModel> diffusionModels = cycleData.Select(anomalyData =>
            {
                decimal diffusion = (anomalyData.Sum(item => 100 - item.Template!.DiffusionSimilarity)) / anomalyData.Count();

                return new CycleDiffusionModel()
                {
                    DiffusionSimilarity = diffusion,
                    Name = "CWIX " + anomalyData.First().Year
                };
            }).ToList();

            // Add empty cycles if required
            for (int i = startYear; i < startYear + cyclesCount; i++)
            {
                string cwixCycleName = "CWIX " + i;

                if (!diffusionModels.Any(value => value.Name == cwixCycleName))
                {
                    diffusionModels.Add(new CycleDiffusionModel() { DiffusionSimilarity = 0, Name = cwixCycleName });
                }
            }

            diffusionModels = diffusionModels.OrderBy(item => item.Name).ToList();

            return diffusionModels;
        }

        public List<FADiffusionModel> GetFocusAreaDiffusions(int startYear, int cyclesCount)
        {
            // Get anomalies for given year.
            var faData = _context.TtYearAnomalies.Include(ta => ta.Template).Include(ta => ta.Fa).ToList().GroupBy(anomaly => anomaly.Fa);


            List<FADiffusionModel> diffusionModels = faData.Select(anomalyData =>
            {
                var anomalyDatas = anomalyData.Select(item => item)
                                              .GroupBy(item => item.Year)
                                              .Select(anomalyGroup =>
                                                  (
                                                      Year: anomalyGroup.First().Year,
                                                      Diffusion: 100 - (anomalyGroup.Sum(item => item.Template.DiffusionSimilarity) / anomalyGroup.Count()))
                                                  );

                return new FADiffusionModel()
                {
                    Name = anomalyData.First().FaName,
                    Values = anomalyDatas.ToList()
                };
            }).ToList();



            // Add empty cycles if required
            for (int i = startYear; i < startYear + cyclesCount; i++)
            {
                foreach (var diffusionModel in diffusionModels)
                {
                    if (!diffusionModel.Values.Any(value => value.Year == i))
                    {
                        diffusionModel.Values.Add((i, 0m));
                    }
                }
            }


            // Return fa diffusion models.
            return diffusionModels;
        }
    }
}
