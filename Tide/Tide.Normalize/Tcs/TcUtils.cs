using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Ef;
using Tide.Data.Models.Tcs;

namespace Tide.Normalize.Tcs
{
    internal static partial class TcUtils
    {
        private static IReadOnlyCollection<Model> Tcs(int year)
        {
            string path = Path.Combine(Context.Path, $@"cwix{year}\tc");
            var files = Directory.EnumerateFiles(path);

            List<Model> tcs = new();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<Model>(text);
                if (model.IsValid)
                {
                    tcs.Add(model);
                }

            }
            return tcs;
        }

        public static TestCase? Get(string code, int year, TideContext context)
        {
            return context.Tests.FirstOrDefault(x => x.Number == code && x.Year == year);
        }

        #region Model 

        private class Model
        {
            [JsonProperty("id")]
            public string Number { get; set; } = string.Empty;

            [JsonProperty("cycle")]
            public int Year { get; set; }

            [JsonProperty("testtemplateid")]
            public string TestTemplateNumber { get; set; } = string.Empty;
            
            [JsonProperty("status")]
            public string Status { get; set; } = string.Empty;

            [JsonProperty("providerids")]
            public string[] Providers { get; set; } = null!;

            [JsonProperty("consumerids")]
            public string[] Consumers { get; set; } = null!;

            //[JsonProperty("mediatorids")]
            //public string[] MediatorIds { get; set; } = null!;

            [JsonProperty("objectiveids")]
            public string[] Objectives { get; set; } = null!;

            [JsonProperty("partialresult")]
            public Partialresult? PartialResult { get; set; }

            [JsonProperty("finalresult")]
            public Finalresult? FinalResult { get; set; }

            public bool IsValid
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(Number)) return false;
                    if (Year < 2014) return false;
                    if (string.IsNullOrWhiteSpace(TestTemplateNumber)) return false;
                    if (string.IsNullOrWhiteSpace(Status)) return false;
                    if(Objectives==null) return false;
                    if (Providers == null) return false;
                    if (Consumers == null) return false;

                    return true;
                }
            }
        }

        private class Partialresult
        {
            [JsonProperty("providers")]
            public ParticipantResult[] Providers { get; set; } = null!;

            [JsonProperty("consumers")]
            public ParticipantResult[] Consumers { get; set; } = null!;

            //[JsonProperty("mediators")]
            //public ParticipantResult[] Mediators { get; set; } = null!;
            public bool IsValid => Providers!= null && Consumers != null;
        }

        private class ParticipantResult
        {
            [JsonProperty("id")]
            public string Id { get; set; } = null!;
           
            [JsonProperty("status")]
            public string Status { get; set; } = null!;

            [JsonProperty("remarks")]
            public string Remarks { get; set; } = null!;
            public bool IsValid => !string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(Status);
        }


        private class Finalresult
        {
            [JsonProperty("result")]
            public string Result { get; set; } = string.Empty;

            [JsonProperty("issuecategory")]
            public string IssueCategory { get; set; } = string.Empty;

            [JsonProperty("shortfallinvestigation")]
            public bool ShortfallInvestigation { get; set; }

            [JsonProperty("remarks")]
            public string? Remarks { get; set; }
        }

        #endregion
    }
}
