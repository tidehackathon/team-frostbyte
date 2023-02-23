using static Tide.Dashboard.Converters.RadarChartModel;

namespace Tide.Dashboard.Converters
{
    public class RadarChartModelConverter<Type>
    {
        private RadarChartModel _modelBuild;

        public RadarChartModelConverter() { 
            _modelBuild= new RadarChartModel();
        }

        public RadarChartModelConverter<Type> AddGroup(List<Type> data, string lineId, Func<Type, RadarChartModel.Data> mappingFunction)
        {
            List<RadarChartModel.Data> radarData = data.Select(mappingFunction).ToList();
            _modelBuild.Groups.Add(new SeriesData()
            {
                Values = radarData,
                Id = lineId,
            });

            return this;
        }

        public RadarChartModel Convert()
        {
            return _modelBuild;             
        }

    }



    public class RadarChartModel
    {
        public class SeriesData
        {
            public string Id { get; set; } = null!;

            public List<Data> Values { get; set; } = null!;
        }

        public class Data
        {

            public decimal Threshold { get; set; }

            public string Label { get; set; } = null!;

            public decimal Value { get; set; }
        }


        public List<SeriesData> Groups { get; set; } = new List<SeriesData>();

    }
}
