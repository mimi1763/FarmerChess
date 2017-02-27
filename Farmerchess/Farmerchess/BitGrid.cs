namespace Farmerchess
{
    internal class BitGrid
    {
        private bool[,] _bitGrid;

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

        //Bit-filters
        private static readonly int HORI = 0x1F;
        private static readonly int VERT = 0x108421;
        private static readonly int DIAGB = 0x1041041; //DIAGonal as Backslash (\)
        private static readonly int DIAGF = 0x111110; //DIAGonal as Forwardslash (/)

        public BitGrid()
        {
            int dimX = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
            int dimY = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountY, true);
            _bitGrid = new bool[dimX, dimY];
            ClearGrid();
        }

        public BitGrid Empty
        {
            get { return new BitGrid(); }
        }

        public void ClearGrid()
        {
            for (var y = 0; y < _bitGrid.GetUpperBound(1); y++)
            {
                for (var x = 0; x < _bitGrid.GetUpperBound(0); x++)
                {
                    _bitGrid[x, y] = false;
                }
            }
        }
    }
}