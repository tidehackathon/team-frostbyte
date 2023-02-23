using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.FA;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Fas;
using Tide.Normalize.Standards;
using static System.Net.Mime.MediaTypeNames;

namespace Tide.Normalize.Tts
{
    internal static partial class TtUtils
    {
        public static void MapObjectives()
        {
            using var context = Context.Db;

            var tests = context.Tests.Include(t => t.Objectives).Include(t => t.Template).AsTracking().ToList();

            foreach (var test in tests)
            {
                // If test contains test template.
                if (test.TemplateId != null)
                {
                    TestTemplateCycle testTemplate = test.Template!;

                    // Retrieve objectives.
                    List<int> objectivesIds = test.Objectives.Select(ob => ob.ObjectiveId)
                        .Where(objId => !testTemplate.Objectives.Any(item => item.ObjectiveId == objId))
                        .ToList();


                    // Add objectives to map of the template.
                    // Get distinct objectives

                    MapObjectives(testTemplate, objectivesIds);
                }
            }



            context.SaveChanges();

            void MapObjectives(TestTemplateCycle testTemplate, List<int> objectiveIds)
            {
                foreach (var objectiveId in objectiveIds)
                {
                    testTemplate.Objectives.Add(new ObjectiveTtMap() { ObjectiveId = objectiveId, TemplateId = testTemplate.Id });
                }
            }

        }

    }
}
