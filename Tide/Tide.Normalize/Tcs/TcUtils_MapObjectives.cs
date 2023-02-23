using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Tcs;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Fas;
using Tide.Normalize.Objectives;

namespace Tide.Normalize.Tcs
{
    internal static partial class TcUtils
    {
        public static void MapObjectives()
        {
            using var context = Context.Db;

            foreach (var folder in Context.Folders)
            {
                var models = Tcs(folder);

                foreach (var model in models)
                {
                    TestCase? testCase = TcUtils.Get(model.Number, model.Year, context);

                    if (testCase != null)
                    {
                        List<ObjectiveCycle> objectives = model.Objectives.Select(item => ObjectiveUtils.Get(item, model.Year, context))
                                                                         .Where(item => item != null)
                                                                         .ToList();

                        testCase.Objectives = objectives.Select(objective => new ObjectiveTcMap() { ObjectiveId = objective.Id, TestId = testCase.Id })
                                .ToList();
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
