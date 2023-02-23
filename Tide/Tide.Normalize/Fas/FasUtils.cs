using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Ef;
using Tide.Data.Models;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Objectives;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Objectives;

namespace Tide.Normalize.Fas
{
    internal  static partial class FasUtils
    {
        private static IReadOnlyCollection<Model> Areas(int year)
        {
            string path = Path.Combine(Context.Path, $@"cwix{year}\fa");
            var files = Directory.EnumerateFiles(path);

            List<Model> areas = new();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<Model>(text);
                if (model != null && !string.IsNullOrWhiteSpace(model.Name) && model.Description != null && model.Capabilities != null && model.Objectives != null)
                {
                    areas.Add(model);
                }

            }
            return areas;
        }

        public static FocusAreaCycle Get(string code,int year, TideContext context)
        {
            return context.FocusAreaCycles.FirstOrDefault(x => x.FocusArea.Name == code && x.Year == year);
        }
        public static void Save()
        {
            Dictionary<string, FocusArea> areas = new();

            foreach (var folder in Context.Folders)
            {
                var models = Areas(folder);
                foreach (var model in models)
                {
                    if (areas.ContainsKey(model.Name))
                    {
                        areas[model.Name].Cicles.Add(new FocusAreaCycle()
                        {
                            Year = model.Cycle
                        });
                    }
                    else
                    {
                        areas.Add(model.Name, new FocusArea()
                        {
                            Name = model.Name,
                            Cicles = new List<FocusAreaCycle>() { new FocusAreaCycle() { Year = model.Cycle } }
                        });
                    }
                }
            }

            using var context = Context.Db;
            context.FocusAreas.AddRange(areas.Values);
            context.SaveChanges();
        }

        public static void MapCapabilities()
        {
            using var context = Context.Db;

            foreach (var folder in Context.Folders)
            {
                var models = Areas(folder);

                foreach (var model in models)
                {
                    FocusAreaCycle? focusArea = FasUtils.Get(model.Name, model.Cycle, context);

                    if (focusArea != null)
                    {
                        List<CapabilityCycle> cycles = model.Capabilities.Select(item => CapabilitiesUtils.Get(item, model.Cycle, context))
                                                                         .Where(item => item != null)
                                                                         .ToList();

                        focusArea.Capabilities = cycles.Select(item => new CapabilityFaMap() { CapabilityId = item.Id, FaId = focusArea.Id }).ToList();
                    }

                }
            }


            context.SaveChanges();
        }

        public static void MapObjectives()
        {
            using var context = Context.Db;

            foreach (var folder in Context.Folders)
            {
                var models = Areas(folder);

                foreach (var model in models)
                {
                    FocusAreaCycle? focusArea = FasUtils.Get(model.Name, model.Cycle, context);

                    if (focusArea != null)
                    {
                        List<ObjectiveCycle> objectives = model.Objectives.Select(item => ObjectiveUtils.Get(item, model.Cycle, context))
                                                                       .Where(item => item != null)
                                                                       .ToList();

                        focusArea.Objectives = objectives.Select(item => new ObjectiveFaMap() { ObjectiveId = item.Id, FaId = focusArea.Id }).ToList();
                    }

                }
            }


            context.SaveChanges();
        }

        private class Model
        {
            public int Cycle { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            [JsonProperty("objectiveids")]
            public List<string> Objectives { get; set; } = new();
            [JsonProperty("capabilitieids")]
            public List<string> Capabilities { get; set; } = new();
        }
    }
}
