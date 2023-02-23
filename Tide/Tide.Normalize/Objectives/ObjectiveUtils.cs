using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Ef;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Standards;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Models;
using Tide.Normalize.Standards;

namespace Tide.Normalize.Objectives
{
    internal static partial class ObjectiveUtils
    {
        public static ObjectiveCycle? Get(string code, int year, TideContext context)
        {
            return context.ObjectiveCycles.FirstOrDefault(x => x.Number == code && x.Year == year);
        }


        public static void MapCapabilities()
        {

            using var context = Context.Db;

            foreach (var folder in Context.Folders)
            {
                var models = Objectives(folder);

                foreach (var model in models)
                {
                    ObjectiveCycle? dbObjective = ObjectiveUtils.Get(model.Number, model.Year, context);

                    if (dbObjective != null)
                    {
                        List<CapabilityCycle> capabilities = model.CapabilitiesIds.Select(item => CapabilitiesUtils.Get(item, model.Year, context))
                                                                            .Where(item => item != null)
                                                                            .ToList();

                        dbObjective.Capabilities = capabilities.Select(item => new ObjectiveCapabilityMap() { CapabilityId = item.Id, ObjectiveId = dbObjective.Id }).ToList();
                    }

                }
            }


            context.SaveChanges();

        }


        public static void Save()
        {
            Dictionary<string, Objective> dbObjectives = new Dictionary<string, Objective>();


            foreach (var folder in Context.Folders)
            {
                var models = Objectives(folder);

                foreach (var model in models)
                {
                    string modelKey = BuildKey(model.Number, model.Year);

                    // Collect keys which may be compatible.
                    string[] matchingKeys = model.Compatibility.Select(compatibility => BuildKey(compatibility.Id, compatibility.Year))
                                                               .ToArray();

                    // Check if any key is already contained by the dbObjectives.
                    string? matchingObjectiveKey = matchingKeys.FirstOrDefault(key => dbObjectives.ContainsKey(key));

                    if (!string.IsNullOrEmpty(matchingObjectiveKey))
                    {
                        Objective matchingObjective = dbObjectives[matchingObjectiveKey];

                        matchingObjective.Cycles.Add(ConvertToObjectiveCycle(model));
                    }
                    else
                    {
                        dbObjectives.Add(modelKey, ConvertToObjective(model));
                    }
                }

            }

            using var context = Context.Db;
            context.Objectives.AddRange(dbObjectives.Values);
            context.SaveChanges();

            Objective ConvertToObjective(Model model)
            {
                var result = new Objective()
                {
                    Cycles = new List<ObjectiveCycle>() { ConvertToObjectiveCycle(model) }
                };

                return result;
            }

            ObjectiveCycle ConvertToObjectiveCycle(Model model)
            {
                var result = new ObjectiveCycle()
                {
                    Year = model.Year,
                    Scope = GetScopeMap(model),
                    Title = model.Title,
                    Number = model.Number
                };

                if (!string.IsNullOrEmpty(model.Description))
                {
                    result.Description = new ObjectiveDescription() { Description = model.Description };
                }


                var dbStandards = StandardsUtils.Get(model.Standards);

                if (dbStandards != null)
                {
                    result.Standards = dbStandards.Select(dbStandard => new StandardObjectiveMap() { StandardId = dbStandard.Id, Objective = result })
                                                  .ToList();
                }

                return result;
            }

            string BuildKey(string code, int year) => $"{code}-{year}";

            ObjectiveScope GetScopeMap(Model model)
            {
                ObjectiveScope result = 0;

                if (model.Scope.exploration) result |= ObjectiveScope.EXPLORATION;
                if (model.Scope.examination) result |= ObjectiveScope.EXAMINATION;
                if (model.Scope.experimentation) result |= ObjectiveScope.EXPERIMENTATION;
                if (model.Scope.exercise) result |= ObjectiveScope.EXERCISE;

                return result;
            }
        }



        private static IReadOnlyCollection<Model> Objectives(int year)
        {
            string path = Path.Combine(Context.Path, $@"cwix{year}\ob");
            var files = Directory.EnumerateFiles(path);

            List<Model> objs = new();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<Model>(text);
                if (model.IsValid)
                {
                    objs.Add(model);
                }

            }
            return objs;
        }

        public class Scope
        {
            public bool exploration { get; set; }
            public bool experimentation { get; set; }
            public bool examination { get; set; }
            public bool exercise { get; set; }
        }

        private class Model
        {
            [JsonProperty("id")]
            public string Number { get; set; } = string.Empty;
            [JsonProperty("cycle")]
            public int Year { get; set; }
            public string Title { get; set; } = string.Empty;
            [JsonProperty("relevantstandards")]
            public List<string> Standards { get; set; } = new();
            public Scope Scope { get; set; } = null!;
            [JsonProperty("objectiveresult")]
            public string Result { get; set; } = string.Empty;
            [JsonProperty("summary")]
            public string Summary { get; set; } = string.Empty;
            public string Recommendation { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            [JsonProperty("compability")]
            public List<Compatibility> Compatibility { get; set; } = new();
            
            [JsonProperty("capabilitiesids")]
            public List<string> CapabilitiesIds { get; set; } = new();

            public bool IsValid
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(Number)) return false;
                    if (Year < 2014) return false;
                    if (string.IsNullOrWhiteSpace(Description)) return false;
                    if (Scope == null) return false;
                    if (string.IsNullOrWhiteSpace(Title)) return false;
                    if (Result == null) return false;
                    if (Summary == null) return false;
                    if (Recommendation == null) return false;
                    if (Description == null) return false;

                    if (Compatibility == null || Compatibility.Any(x => !x.IsValid)) return false;
                    return true;
                }
            }
        }
    }
}
