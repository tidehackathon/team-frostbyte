using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Objectives
{
    public class ObjectiveTtMap
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int ObjectiveId { get; set; }

        public ObjectiveCycle? Objective { get; set; }
        public TestTemplateCycle? Template { get; set; }
    }
}
