using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Tts;

namespace Tide.Data.Models.Charts.Anomalies
{
    public class TtYearAnomaly
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; } = string.Empty;
        public int FaId { get; set; }
        public string FaName { get; set; } = string.Empty;
        public int Year { get; set; }

        public TestTemplateCycle? Template { get; set; }
        public ObjectiveCycle? Objective { get; set; }
        public FocusArea? Fa { get; set; }
    }
}
