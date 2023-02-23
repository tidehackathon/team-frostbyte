using static Tide.Dashboard.Converters.StackedDrawerModel;

namespace Tide.Dashboard.Converters
{
    public class StackedDrawerModelConverter<Type>
    {
        private StackedDrawerModel _modelBuild;

        public StackedDrawerModelConverter() { 
            _modelBuild= new StackedDrawerModel();
        }

        public StackedDrawerModelConverter<Type> AddGroup(List<Type> data, string lineId, Func<Type, StackedDrawerModel.Data> mappingFunction)
        {
            List<StackedDrawerModel.Data> polarData = data.Select(mappingFunction).ToList();
            _modelBuild.Lines.Add(new SeriesData()
            {
                Data = polarData,
                Id = lineId,
            });

            return this;
        }

        public StackedDrawerModel Convert()
        {
            return _modelBuild;             
        }

    }



    public class StackedDrawerModel
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
