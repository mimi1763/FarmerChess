using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    class BitLine
    {
        /*
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
        public int StartCell { get; private set; }
        public int EndCell { get; private set; }
        public LineShape Shape { get; private set; }
        public int Length { get; private set; }

        public enum LineShape
        {
            Horizontal,
            Vertical,
            SlashLeft,
            SlashRight
        }

        public BitLine(int start, int end, LineShape shape)
        {
            StartCell = start;
            EndCell = end;
            Shape = shape;
            CalculateLength();
        }

        private void CalculateLength()
        {
            
        }
    }
}
