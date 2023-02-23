using static Tide.Dashboard.Converters.ChordDiagramModel;

namespace Tide.Dashboard.Converters
{
    public class ChordDiagramModelConverter<Type>
    {
        private readonly ChordDiagramModel _model;


       public  ChordDiagramModelConverter()
        {
            _model = new ChordDiagramModel();
        }

        public ChordDiagramModelConverter<Type> AddConnections(IEnumerable<Type> dataModels, Func<Type, Connection> mappingFunction)
        {
            var connections = dataModels.Select(model => mappingFunction(model));

            _model.Connections.AddRange(connections);

            return this;
        }

        public ChordDiagramModel Convert() => _model;
    }

    public class ChordDiagramModel
    {
        public class Connection
        {
            public string From { get; set; } = null!;

            public string To { get; set; } = null!;

            public decimal Value { get; set; }

            public string ValueLabel { get; set; } = string.Empty;

            public List<AdditionalText> AdditionalText { get; set; } = new List<AdditionalText>();
        }

        public class AdditionalText
        {
            public string Label { get; set; } = string.Empty;

            public decimal Value { get; set; }
        }

        public List<Connection> Connections { get; set; } = new List<Connection>();
    }
}
