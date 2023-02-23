using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Nato;

namespace Tide.Normalize.Nato
{
    internal static class NdppUtils
    {
        private static IReadOnlyCollection<Model> Capabilities()
        {
            string path = Path.Combine(Context.Path, $@"npp\capabilities.json");

            string text = File.ReadAllText(path);
            var model = JsonConvert.DeserializeObject<List<Model>>(text).Where(x=>x.IsValid).ToList();

            return model;
        }

        public static void Save()
        {
            using var context = Context.Db;
            var capabilities = Capabilities().Select(x => new Ndpp() { Code = x.code, Description = x.description, Domain = x.operational_area }).ToList();
            context.Ndpps.AddRange(capabilities);
            context.SaveChanges();
        }


        public class Model
        {
            public string code { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public string operational_area { get; set; } = string.Empty;

            public bool IsValid
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(code)) return false;
                    if (string.IsNullOrWhiteSpace(description)) return false;
                    if (string.IsNullOrWhiteSpace(operational_area)) return false;
                    return true;
                }
            }
        }

    }
}
