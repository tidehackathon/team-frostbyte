using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models.FA;

namespace Tide.Explorer
{
    internal class FaMdModel
    {
        public FaMdModel(int from, int to)
        {
            From = from;
            To = to;
        }

        public int From { get; private init; }
        public int To { get; private init; }

        public int Count { get; set; }
        public int Success { get; set; }
        public int Failure { get; set; }
    }
}
