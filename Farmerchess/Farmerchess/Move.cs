using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    public class Move
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tools.Player Colour { get; set; }
    }
}
