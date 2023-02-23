using System;
namespace Tide.Dashboard.Converters
{
	public class HeatMapAxesModelConvert
	{
		HeatMapAxesModel _model;
		public HeatMapAxesModelConvert()
		{
            _model = new HeatMapAxesModel();
		}
        public HeatMapAxesModelConvert AddData(HeatMapAxesModel.DataObject dataObject)
        {
            this._model.data.Add(dataObject);
            return this;
        }
        public HeatMapAxesModelConvert AddData(HeatMapAxesModel.TagObject dataObject)
        {
            this._model.tags.Add(dataObject);
            return this;
        }
        public HeatMapAxesModelConvert AddData(HeatMapAxesModel.CircleItemObject dataObject)
        {
            this._model.circleItems.Add(dataObject);
            return this;
        }
        public HeatMapAxesModel Convert()
        {
            return _model;
        }
    }
    public class HeatMapAxesModel
    {
        public class DataObject
        {
            public string Tag { get; set; } = null!;
            public string CircleItem { get; set; } = null!;
            public decimal Value { get; set; }
        }
        public class TagObject
        {
            public string Tag { get; set; } = null!;
        }
        public class CircleItemObject
        {
            public string CircleItem { get; set; } = null;
        }

        public List<DataObject> data { get; set; } = new List<DataObject>();
        public List<TagObject> tags { get; set; } = new List<TagObject>();
        public List<CircleItemObject> circleItems { get; set; } = new List<CircleItemObject>();
    }
}

