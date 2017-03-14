﻿using Farmerchess.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;

namespace Farmerchess
{
    internal class BitGrid
    {
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

            Tests:

            4 20 21 22 23 24
            3 15 16 17 18 19
            2 10 11 12 13 14
            1  5  6  7  8  9
            0  0  1  2  3  4
               a  b  c  d  e

            4  1  0  0  0  0
            3  0  1  1  1  0
            2  0  1  0  0  0
            1  0  1  0  1  1
            0  0  0  1  0  0
               a  b  c  d  e

            = 00001 01110 00010 11010 00100

            9 90 91 92 93 94 95 96 97 98 99
            8 80 81 82 83 84 85 86 87 88 89
            7 70 71 72 73 74 75 76 77 78 79
            6 60 61 62 63 64 65 66 67 68 69
            5 50 51 52 53 54 55 56 57 58 59
            4 40 41 42 43 44 45 46 47 48 49
            3 30 31 32 33 34 35 36 37 38 39
            2 20 21 22 23 24 25 26 27 28 29
            1 10 11 12 13 14 15 16 17 18 19
            0  0  1  2  3  4  5  6  7  8  9
               a  b  c  d  e  f  g  h  i  j
        */

        private BigInteger _bitGrid;
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public Tools.Player Player { get; private set; }

        //Test grid 10x10
        private static int[,] _testGridO = new int[,]  { { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                         { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }};

        private static int[,] _testGridX = new int[,]  { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        //Bit-filters  <------------------ PS: These values are wrong now! Fix!
        private static readonly int HORI = 0x1F;
        private static readonly int VERT = 0x108421;
        private static readonly int DIAGB = 0x1041041; //DIAGonal as Backslash (\)
        private static readonly int DIAGF = 0x111110; //DIAGonal as Forwardslash (/)

        public BitGrid(Tools.Player player, Board board, bool useTestGrid = false)
        {
            Player = player;

            SizeX = board.BlockCountX;
            SizeY = SizeX; //Always square boards!
            
            _bitGrid = useTestGrid ? (player == Tools.Player.O ? ConvertFromIntArray(_testGridO) : ConvertFromIntArray(_testGridX)) : new BigInteger(SizeX * SizeY);
        }

        public BigInteger ConvertFromIntArray(int[,] intArray)
        {
            BigInteger bitGrid = new BigInteger((intArray.GetUpperBound(0) + 1) ^ 2);

            bool bit = false;
            for (var y = 0; y < SizeY; y++)
            {
                for (var x = 0; x < SizeX; x++)
                {
                    bit = intArray[x, y] != (int)Tools.Player.Empty;
                    SetBit(ref bitGrid, y * SizeY + x, bit);
                }
            }

            return bitGrid;
        }

        private void SetBit(ref BigInteger bitGrid, int bitPos, bool value = true)
        {
            bitGrid = value ? bitGrid | (1 << bitPos) : bitGrid & ~(1 << bitPos);
        }

        private bool GetBit(BigInteger bitGrid, int bitPos)
        {
            return (bitGrid & (1 << bitPos)) != 0;
        }

        public int GetPlayerInt()
        {
            return (int)Player;
        }

        public void ClearGrid()
        {
            _bitGrid = 0;
        }

        public bool CheckRows(int noInARow, int lastPosPlayed)
        {
            int row = lastPosPlayed / SizeY; 
            // bit positions = row * sizeY -> row * sizeY + sizeX


            return false;
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

        public static int TestGridSize
        {
            get { return _testGridO.GetUpperBound(0) + 1; }
        }
    }
}