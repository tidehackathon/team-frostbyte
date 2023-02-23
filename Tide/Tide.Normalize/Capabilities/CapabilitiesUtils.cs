using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Ef;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Standards;
using Tide.Normalize.Countries;
using Tide.Normalize.Models;
using Tide.Normalize.Standards;

namespace Tide.Normalize.Capabilities
{
    internal static partial class CapabilitiesUtils
    {
        private static readonly Maturity DEFAULT_MATURITY = Maturity.EXPERIMENTAL;
        
        public static CapabilityCycle? Get (string number,int year,TideContext context)
        {
            return context.CapabilityCicles.FirstOrDefault(x => x.Number == number && x.Year == year);
        }
        //private static 
        private static IReadOnlyCollection<Model> Capabilities(int year)
        {
            string path = Path.Combine(Context.Path, $@"cwix{year}\cc");
            var files = Directory.EnumerateFiles(path);

            List<Model> ccs = new() { Capacity = files.Count() + 1 };

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<Model>(text);
                if (model.IsValid)
                    ccs.Add(model);
            }
            return ccs;
        }

        public static void Test()
        {
            foreach (var folder in Context.Folders)
            {
                var ccs = Capabilities(folder);
                foreach (var cc in ccs)
                {
                    var standards = StandardsUtils.Get(cc.Standards);
                    if (standards.Count != cc.Standards.Count)
                    {
                        Console.WriteLine("--------------------------------");
                        Console.WriteLine(cc.Number);
                        Console.WriteLine(string.Join("/", standards.Select(x => x.Name)));
                        Console.WriteLine(string.Join("/", cc.Standards));
                        Console.WriteLine("--------------------------------");
                    }

                }
            }
            Console.WriteLine("No problems");
        }

        /// <summary>
        /// Check if maturity contained by capabilities are valid.
        /// </summary>
        internal static void TestMaturityStrings()
        {
            foreach (var folder in Context.Folders)
            {
                var models = Capabilities(folder);

                foreach (var model in models)
                {
                    Maturity? maturity = ConvertMaturityFromString(model.Maturity);

                    if (maturity == null)
                    {
                        Console.WriteLine($"Couldn't parse {model.Maturity}");
                    }
                }
            }
        }

        internal static void Save()
        {
            Dictionary<string, Capability> dbCapabilities = new Dictionary<string, Capability>();

            foreach (var folder in Context.Folders)
            {
                var models = Capabilities(folder);

                foreach (var model in models)
                {
                    string modelKey = BuildKey(model.Number, model.Year);

                    // Collect keys which may be compatible.
                    string[] matchingKeys = model.Compatibility.Select(compatibility => BuildKey(compatibility.Id, compatibility.Year))
                                                               .ToArray();

                    // Check if any key is already contained by the dbCapabilities.
                    string? matchingCapabilityKey = matchingKeys.FirstOrDefault(key => dbCapabilities.ContainsKey(key));

                    if (!string.IsNullOrEmpty(matchingCapabilityKey))
                    {
                        Capability matchingCapability = dbCapabilities[matchingCapabilityKey];

                        matchingCapability.Cycles.Add(ConvertToCycle(model));
                    }
                    // If no key available, remove 
                    else
                    {
                        dbCapabilities.Add(modelKey, ConvertToCapability(model));
                    }
                }
            }


            using var context = Context.Db;
            context.Capabilities.AddRange(dbCapabilities.Values);
            context.SaveChanges();


            string BuildKey(string id, int year) => $"{id}-{year}";

            Capability ConvertToCapability(Model model)
            {
                return new Capability()
                {
                    Name = model.Name,
                    NationId = CountryUtils.Contains(model.Country) ? CountryUtils.Get(model.Country).Id : 0,
                    Cycles = new List<CapabilityCycle>() { ConvertToCycle(model) }
                };
            }

            CapabilityCycle ConvertToCycle(Model model)
            {
                var result = new CapabilityCycle()
                {
                    Number = model.Number,
                    Year = model.Year,
                    Maturity = ConvertMaturityFromString(model.Maturity) ?? DEFAULT_MATURITY
                };

                if (!string.IsNullOrEmpty(model.Description))
                {
                    result.Description = new CapabilityDescription()
                    {
                        Description = model.Description
                    };
                }

                var dbStandards = StandardsUtils.Get(model.Standards);

                if (dbStandards != null)
                {
                    result.Standards = dbStandards.Select(dbStandard => new StandardCapabilityMap() { StandardId = dbStandard.Id, Capability = result })
                                                  .ToList();
                }

                return result;
            }


        }

        public static Maturity? ConvertMaturityFromString(string maturityString) => maturityString switch
        {
            "Fielded" => Maturity.FIELDED,
            "Experimental" => Maturity.EXPERIMENTAL,
            "Developmental" => Maturity.DEVELOPMENTAL,
            "Near-Fielded" => Maturity.NEAR_FIELDED,
            _ => null
        };

        
        private class Model
        {
            [JsonProperty("id")]
            public string Number { get; set; } = string.Empty;
            [JsonProperty("cycle")]
            public int Year { get; set; }
            public string Maturity { get; set; } = string.Empty;
            [JsonProperty("operationaldomains")]
            public List<string> Domains { get; set; } = new();
            [JsonProperty("tasks")]
            public List<string> Tasks { get; set; } = new();
            [JsonProperty("standards")]
            public List<string> Standards { get; set; } = new();
            [JsonProperty("interoperabilityachievements")]
            public List<string> Achievements { get; set; } = new();
            [JsonProperty("interoperabilityimprovements")]
            public List<string> Imporvements { get; set; } = new();
            [JsonProperty("interoperabilitychallenges")]
            public List<string> Challenges { get; set; } = new();
            public string Country { get; set; } = string.Empty;
            public List<Compatibility> Compatibility { get; set; } = new();
            public bool Withdrawn { get; set; }

            public string Description { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public bool IsValid
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(Number)) return false;
                    if (Year < 2014) return false;
                    if (string.IsNullOrWhiteSpace(Maturity)) return false;
                    if (string.IsNullOrWhiteSpace(Description)) return false;
                    if (string.IsNullOrWhiteSpace(Name)) return false;
                    if (Domains == null || Domains.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (Tasks == null || Tasks.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (Standards == null || Standards.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (Achievements == null || Achievements.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (Imporvements == null || Imporvements.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (Challenges == null || Challenges.Any(x => string.IsNullOrWhiteSpace(x))) return false;
                    if (string.IsNullOrWhiteSpace(Country)) return false;
                    if (Compatibility == null || Compatibility.Any(x => !x.IsValid)) return false;
                    if (Withdrawn) return false;
                    return true;
                }
            }

        }
    }
}
