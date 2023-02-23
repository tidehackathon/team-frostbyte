using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Fas;
using Tide.Normalize.Standards;

namespace Tide.Normalize.Tts
{
    internal static partial class TtUtils
    {
        public static void Save()
        {
            using var context = Context.Db;
            List<TestTemplate> templates= new List<TestTemplate>();
            Dictionary<string, TestTemplateCycle> dbCycles = new ();

            foreach (var folder in Context.Folders)
            {
                var models = Tts(folder);

                foreach (var model in models)
                {
                    string key = BuildKey(model.Number, model.Year);

                    if (model.Timeline.Count > 0)
                    {
                        var snapshot = model.Timeline.FirstOrDefault(x => dbCycles.ContainsKey(BuildKey(x.Id, x.Year)));
                        if (snapshot != null)
                        {
                            var tt = dbCycles[BuildKey(snapshot.Id, snapshot.Year)].Template!;
                           var cycle=ConvertToCycle(tt,model, snapshot.Year);
                            dbCycles.Add(BuildKey(cycle.Number, cycle.Year), cycle);
                        }
                        else
                        {
                            CreateTemplate(model);
                        }
                    }
                    else
                    {
                        if(model.Diffusion==null)
                            CreateTemplate(model);
                        else
                        {
                            string diffKey = BuildKey(model.Diffusion.Id, model.Year);
                            if(dbCycles.ContainsKey(diffKey))
                            {
                                ConvertToCycle(dbCycles[diffKey].Template!,model, diffusion: dbCycles[diffKey]);
                            }
                            else
                            {
                                int i = 0;
                                i++;
                            }
                        }
                    }

                }

            }

            int limit = templates.Count / 200 + (templates.Count % 200 == 0 ? 0 : 1);
            for (int i=0;i< limit; i++)
            {
                var chunk = templates.Skip(i * 200).Take(200).ToList();
                context.Templates.AddRange(chunk);
                context.SaveChanges();
            }

            string BuildKey(string id, int year) => $"{id}-{year}";

            TestTemplateCycle ConvertToCycle(TestTemplate template, Model model,int year=0, TestTemplateCycle? diffusion=null) {
                var cycle= new TestTemplateCycle()
                {
                    Number = model.Number,
                    Year = model.Year,
                };

                if(model.Diffusion!=null && diffusion!=null && model.Diffusion.Id==diffusion.Number)
                {
                    cycle.Diffusion = diffusion;
                    diffusion.Duplicates.Add(cycle);
                    cycle.DiffusionSimilarity=model.Diffusion.Similarity;
                }
                if (year > 0 && model.Timeline.Any(x => x.Year == year))
                    cycle.Similarity = model.Timeline.First(x => x.Year == year).Similarity;

                template.Cycles.Add(cycle);
                cycle.Template = template;

                return cycle;
            }


            TestTemplate ConvertToTestTemplate(Model model)
            {
                var result = new TestTemplate()
                {
                    Description= new TestTemplateDescription()
                    {
                        Purpose = model.Purpose,
                        Preconditions = model.PreCondition,

                    },
                    Result = new TestTemplateResult()
                    {
                        Interoperability = model.InteroperabilityIssue,
                        Limited = model.LimitedSuccess,
                        Success = model.Success,                        
                    },                    
                };

                ConvertToCycle(result, model);

                var dbStandards = StandardsUtils.Get(model.Standards);

                if (dbStandards != null)
                {
                    result.Standards = dbStandards.Select(dbStandard => new StandardTtMap() { StandardId = dbStandard.Id, TestTemplate = result })
                                                  .ToList();
                }

                return result;
            }

            TestTemplate CreateTemplate(Model model)
            {
                TestTemplate template = ConvertToTestTemplate(model);
                var cycle = template.Cycles.First();

                dbCycles.Add(BuildKey(model.Number,model.Year), cycle);
                templates.Add(template);
                return template;
            }
        }

    }
}
