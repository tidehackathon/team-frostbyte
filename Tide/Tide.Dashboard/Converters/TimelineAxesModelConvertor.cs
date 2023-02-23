using System;
namespace Tide.Dashboard.Converters
{
	public class TimelineAxesModelConvertor<Type>
	{
		private TimelineAxesModel _model;

		public TimelineAxesModelConvertor()
		{
			_model = new TimelineAxesModel();
		}

		public TimelineAxesModelConvertor<Type> AddGroup(List<Type> data,string Name,Func<Type,TimelineAxesModel.Object> mappingFunction)
		{
			List<TimelineAxesModel.Object> values = data.Select(mappingFunction).ToList();
			_model.Series.Add(new TimelineAxesModel.Data()
			{
				Name = Name,
				Values = values
			});
			return this;
		}
		public TimelineAxesModelConvertor<Type> SetEndAndStart(int yearStart,int yearEnd)
		{
			_model.YearStart = yearStart;
			_model.YearEnd = yearEnd;
			return this;
		}

		public TimelineAxesModel Convert()
		{
			return _model;
		}
	}
	public class TimelineAxesModel
	{
		public int YearStart { get; set; }
		public int YearEnd { get; set; }

		public class Data
		{
		public string Name { get; set; } = null!;
		public List<Object> Values { get; set; }
		}
		public List<Data> Series { get; set; } = new List<Data>();


		public class Object
		{
			public decimal Value { get; set; }
			public string Year { get; set; } = null!;
		}


	}
	
}

