using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess.Gui
{
    class Cell
    {
        public Cell(int posx, int posy, int value)
        {
            PosX = posx;
            PosY = posy;
            Value = value;
        }
        public int Value { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}
