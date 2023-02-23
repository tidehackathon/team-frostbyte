using System;
using static Tide.Dashboard.Converters.NetworkAxesModel;

namespace Tide.Dashboard.Converters
{
    public class NeuronalAxesModelConvertor
    {

        private NetworkAxesModel _model;

        public NeuronalAxesModelConvertor()
        {
            _model = new NetworkAxesModel();
        }

        public NodeModel AddNode(string name, string description, decimal value, string id, params string[] links)
        {
            var node = new NodeModel()
            {
                Description = description,
                Name = name,
                Value = value,
                LinkWith = links.ToList(),
                Id = id,
            };

            _model.Data.Add(node);

            return node;
        }

        public NetworkAxesModel Convert()
        {
            return _model;
        }


    }
    public class NetworkAxesModel
    {


        public List<NodeModel> Data { get; set; } = new List<NodeModel>();

        public class NodeModel
        {
            public string Id { get; set; } = null;

            public string Name { get; set; } = null!;
            public string Description { get; set; } = null;
            public decimal Value { get; set; }
            public List<NodeModel> Children { get; set; } = new List<NodeModel>();
            public List<string>? LinkWith { get; set; }

            public NodeModel AddChild(string name, string description, decimal value)
            {
                var newModel = new NodeModel() { Description = description, Value = value, Name = name };

                this.Children.Add(newModel);

                return newModel;
            }
        }


    }
}

