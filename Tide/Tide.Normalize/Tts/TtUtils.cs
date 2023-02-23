using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Ef;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;

namespace Tide.Normalize.Tts
{
    internal static partial class TtUtils
    {
        private static IReadOnlyCollection<Model> Tts(int year)
        {
            string path = Path.Combine(Context.Path, $@"cwix{year}\tt");
            var files = Directory.EnumerateFiles(path);

            List<Model> tts = new();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<Model>(text);
                if (model.IsValid)
                {
                    tts.Add(model);
                }
                else
                {
                    int x = 0;
                    x++;
                }

            }
            tts=tts.OrderBy(x => x.Diffusion != null).ToList();
            return tts;
        }

        public static TestTemplateCycle? Get(string code, int year, TideContext context)
        {
            return context.TemplateCycles.FirstOrDefault(x => x.Number == code && x.Year == year);
        }

        private class Model
        {
            [JsonProperty("id")]
            public string Number { get; set; } = string.Empty;
            [JsonProperty("cycle")]
            public int Year { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; } = string.Empty;
            [JsonProperty("purpose")]
            public string Purpose { get; set; } = string.Empty;
            [JsonProperty("precondition")]
            public string PreCondition { get; set; } = string.Empty;
            [JsonProperty("steps")]
            public List<Step> Steps { get; set; } = new();
            [JsonProperty("successcriteria")]
            public string Success { get; set; } = string.Empty;
            [JsonProperty("limitedsuccesscriteria")]
            public string LimitedSuccess { get; set; } = string.Empty;
            [JsonProperty("interopissuecriteria")]
            public string InteroperabilityIssue { get; set; } = string.Empty;
            [JsonProperty("standards")]
            public List<string> Standards { get; set; } = new();
            public List<Snapshot> Timeline { get; set; } = new();
            public Diffusion? Diffusion { get; set; }
            public bool IsValid
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(Number)) return false;
                    if (Year < 2014) return false;
                    if (string.IsNullOrWhiteSpace(Status)) return false;
                    if (Success==null) return false;
                    if (LimitedSuccess == null) return false;
                    if (InteroperabilityIssue == null) return false;
                    if (Purpose == null) return false;
                    if (PreCondition == null) return false;
                    if (Steps == null) return false;
                    if (Standards == null) return false;
                    if (Diffusion != null && !Diffusion.IsValid) return false;
                    if (Timeline == null || Timeline.Any(x => !x.IsValid)) return false;
                    Timeline = Timeline.GroupBy(x => x.Year).Select(x => x.First()).ToList();
                    return true;
                }
            }
        }

        private class Step
        {
            [JsonProperty("order")]
            public int Order { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; } = string.Empty;
            [JsonProperty("expectedresult")]
            public string Expected { get; set; } = string.Empty;
            public bool IsValid => Order > 0 && Description != null && Expected != null;
        }

        private class Diffusion
        {
            public string Id { get; set; } = string.Empty;
            public decimal Similarity { get; set; }
            public bool IsValid => !string.IsNullOrWhiteSpace(Id);
        }

        private class Snapshot
        {
            public string Id { get; set; } = string.Empty;
            public decimal Similarity { get; set; }
            [JsonProperty("cycle")]
            public int Year { get; set; }
            public bool IsValid => !string.IsNullOrWhiteSpace(Id);
        }
    }
}
