using Farmerchess.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;

namespace Farmerchess
{
    internal class BitGrid
    {
        private BigInteger _bitGrid;
        
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public Tools.Player Player { get; private set; }

        /* 
        (Rank, row)
                 -- -- -- -- --
           4    |20|21|22|23|24|
                 -- -- -- -- --
           3    |15|16|17|18|19|
                 -- -- -- -- --
           2    |10|11|12|13|14|
                 -- -- -- -- --
           1    | 5| 6| 7| 8| 9|
                 -- -- -- -- --
           0    | 0| 1| 2| 3| 4|
                 -- -- -- -- --
                  a  b  c  d  e  (File, column)
           
        Ex: c3 = (starting from 0) column 2, row 3

            ------------------ PS: This is wrong now! Fix!
            HORI = A 0-4 = bit 1-5 = 11111 = 0x1F
            VERT = A0-E0 = bits 1,6,11,16,21 = 100001000010000100001 = 0x108421
            DIAGB = A0,B1,C2,D3,E4 = bits 1,7,13,19,25 = 1000001000001000001000001 = 0x1041041
            DIABF = A4,B8,C12,D16,E20 = bits 5,9,13,17,21 = 100010001000100010000 = 0x111110

            Shifting:
            --------------------------------
              northwest    north   northeast
              noWe         nort         noEa
                      +7    +8    +9
                          \  |  /
              west    -1 <-  0 -> +1    east
                          /  |  \
                      -9    -8    -7
              soWe         sout         soEa
              southwest    south   southeast

            For 8x8 boards:
            -----------------------------------------------------------------
            const U64 notAFile = 0xfefefefefefefefe; // ~0x0101010101010101
            const U64 notHFile = 0x7f7f7f7f7f7f7f7f; // ~0x8080808080808080

            U64 eastOne (U64 b) {return (b << 1) & notAFile;}
            U64 noEaOne (U64 b) {return (b << 9) & notAFile;}
            U64 soEaOne (U64 b) {return (b >> 7) & notAFile;}
            U64 westOne (U64 b) {return (b >> 1) & notHFile;}
            U64 soWeOne (U64 b) {return (b >> 9) & notHFile;}
            U64 noWeOne (U64 b) {return (b << 7) & notHFile;}
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

        //Bit-filters  <------------------ PS: These values are wrong now! Fix!
        private static readonly int HORI = 0x1F;
        private static readonly int VERT = 0x108421;
        private static readonly int DIAGB = 0x1041041; //DIAGonal as Backslash (\)
        private static readonly int DIAGF = 0x111110; //DIAGonal as Forwardslash (/)

        public BitGrid(Tools.Player player, bool useTestGrid = false)
        {
            Player = player;

            SizeX = useTestGrid ? _testGrid.GetUpperBound(0) + 1 : (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
            SizeY = SizeX; //Always square boards!
            
            _bitGrid = useTestGrid ? ConvertFromIntArray(_testGrid) : new BigInteger(SizeX * SizeY);
            _bitList = new List<BitArray>();
            for (var y = 0; y < SizeY; y++)
            {
                bool[] bits = new bool[SizeX];
                for (var x = 0; x < SizeX; x++)
                {
                    bits[x] = GetGridPiece(x, y) == Tools.Player.Empty ? false : GetGridPiece(x, y) == Tools.Player.X ? true : true;
                }
                _bitList.Add(new BitArray(bits));
            }
        }

        public BigInteger ConvertFromIntArray(int[,] intArray)
        {
            BigInteger bitGrid = new BigInteger((intArray.GetUpperBound(0) + 1) ^ 2);

            for (var y = 0; y < SizeY; y++)
            {
                bool[] bits = new bool[SizeX];
                for (var x = 0; x < SizeX; x++)
                {
                    bits[x] = GetGridPiece(x, y) == Tools.Player.Empty ? false : GetGridPiece(x, y) == Tools.Player.X ? true : true;
                }
                _bitList.Add(new BitArray(bits));
            }

            return bitGrid;
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

        public Tools.Player GetGridPiece(int x, int y)
        {
            switch (_bitGrid[x, y])
            {
                case 0:     return Tools.Player.Empty;
                case 1:     return Tools.Player.X;
                case 2:     return Tools.Player.O;
                default:    return Tools.Player.Empty;
            } 
        }

        /// <summary>
        /// This evaluation method needs lots of work!
        /// </summary>
        /// <returns></returns>
        public int Evaluate()
        {
            int score = 0;

            return score;
        }

        public static Size TestGridSize
        {
            get { return new Size(_testGrid.GetUpperBound(0) + 1, _testGrid.GetUpperBound(1) + 1); }
        }
    }
}