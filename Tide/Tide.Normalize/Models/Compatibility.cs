using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Normalize.Models
{
    internal class Compatibility
    {
        public string Id { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsValid =>!string.IsNullOrWhiteSpace(Id)&& Year>=2014;
    }
}
