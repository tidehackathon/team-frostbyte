﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.Capabilities;

namespace Tide.Data.Models
{
    public class Nation
    {
        public Nation()
        {
            Capabilities = new List<Capability>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public ICollection<Capability> Capabilities { get; set; }
    }
}
