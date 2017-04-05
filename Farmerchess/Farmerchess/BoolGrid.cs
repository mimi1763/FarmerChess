using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    public class BoolGrid
    {
        public bool[,] Grid { get; set; }

        public int Size { get; private set; }

        public BoolGrid(int size)
        {
            Size = size;
            Grid = new bool[Size, Size];
        }

        public static BoolGrid operator |(BoolGrid g1, BoolGrid g2)
        {
            return BaseOperator(g1, g2, Operator.OR);
        }

        public static BoolGrid operator &(BoolGrid g1, BoolGrid g2)
        {
            return BaseOperator(g1, g2, Operator.AND);
        }

        public static BoolGrid operator ^(BoolGrid g1, BoolGrid g2)
        {
            return BaseOperator(g1, g2, Operator.XOR);
        }

        private static BoolGrid BaseOperator(BoolGrid g1, BoolGrid g2, Operator sign)
        {
            BoolGrid grid = new BoolGrid(g1.Size);
            for (var y = 0; y < g1.Size; y++)
            {
                for (var x = 0; x < g1.Size; x++)
                {
                    switch (sign)
                    {
                        case Operator.OR:
                            grid.Grid[x, y] = g1.Grid[x, y] | g2.Grid[x, y];
                            break;
                        case Operator.AND:
                            grid.Grid[x, y] = g1.Grid[x, y] & g2.Grid[x, y];
                            break;
                        case Operator.XOR:
                            grid.Grid[x, y] = g1.Grid[x, y] ^ g2.Grid[x, y];
                            break;
                    }
                }
            }
            return grid;
        }

        private enum Operator
        {
            OR,
            AND,
            XOR
        }
    }
}
