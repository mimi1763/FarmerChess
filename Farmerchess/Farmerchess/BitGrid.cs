using Farmerchess.Gui;
using System.Windows;

namespace Farmerchess
{
    internal class BitGrid
    {
        private int[,] _bitGrid;

        /*
             -- -- -- -- --
           A| 0| 1| 2| 3| 4|
             -- -- -- -- --
           B| 5| 6| 7| 8| 9|
             -- -- -- -- --
           C|10|11|12|13|14|
             -- -- -- -- --
           D|15|16|17|18|19|
             -- -- -- -- --
           E|20|21|22|23|24|
             -- -- -- -- --
              0  1  2  3  4

            = max 25 bits = int.
            HORI = A 0-4 = bit 1-5 = 11111 = 0x1F
            VERT = A0-E0 = bits 1,6,11,16,21 = 100001000010000100001 = 0x108421
            DIAGB = A0,B1,C2,D3,E4 = bits 1,7,13,19,25 = 1000001000001000001000001 = 0x1041041
            DIABF = A4,B8,C12,D16,E20 = bits 5,9,13,17,21 = 100010001000100010000 = 0x111110
        */

        //Test grid 10x10
        private static int[,] _testGrid= new int[,]  {  { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                                        { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                        { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                        { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
                                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                        { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
                                                        { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                        { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }};

        //Bit-filters
        private static readonly int HORI = 0x1F;
        private static readonly int VERT = 0x108421;
        private static readonly int DIAGB = 0x1041041; //DIAGonal as Backslash (\)
        private static readonly int DIAGF = 0x111110; //DIAGonal as Forwardslash (/)

        public BitGrid(bool useTestGrid = false)
        {
            int dimX = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
            int dimY = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountY, true);
            _bitGrid = useTestGrid ? _testGrid : new int[dimX, dimY];
        }

        public void ClearGrid()
        {
            for (var y = 0; y < _bitGrid.GetUpperBound(1); y++)
            {
                for (var x = 0; x < _bitGrid.GetUpperBound(0); x++)
                {
                    _bitGrid[x, y] = 0;
                }
            }
        }

        public Board.Player GetGridPiece(int x, int y)
        {
            switch (_bitGrid[x, y])
            {
                case 0:     return Board.Player.Empty;
                case 1:     return Board.Player.X;
                case 2:     return Board.Player.O;
                default:    return Board.Player.Empty;
            } 
        }

        public static Size TestGridSize
        {
            get { return new Size(_testGrid.GetUpperBound(0) + 1, _testGrid.GetUpperBound(1) + 1); }
        }
    }
}