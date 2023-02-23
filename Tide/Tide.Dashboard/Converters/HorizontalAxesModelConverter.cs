using static Tide.Dashboard.Converters.HorizontalAxesModel;

namespace Tide.Dashboard.Converters
{
    public class HorizontalAxesModelConverter<Type>
    {
        private HorizontalAxesModel _modelBuild;

        public HorizontalAxesModelConverter() { 
            _modelBuild= new HorizontalAxesModel();
        }

        public HorizontalAxesModelConverter<Type> AddGroup(List<Type> data, string lineId, Func<Type, HorizontalAxesModel.Data> mappingFunction)
        {
            List<HorizontalAxesModel.Data> polarData = data.Select(mappingFunction).ToList();
            _modelBuild.Lines.Add(new SeriesData()
            {
                Data = polarData,
                Id = lineId,
            });

            return this;
        }

        public HorizontalAxesModel Convert()
        {
            return _modelBuild;             
        }

    }



    public class HorizontalAxesModel
    {
        public class SeriesData
        {
            public string Id { get; set; }

            public List<Data> Data { get; set; }
        }

        public class Data
        {

            public decimal X { get; set; }

            public string Y { get; set; } = null!;
        }


        public List<SeriesData> Lines { get; set; } = new List<SeriesData>();

    }
}
