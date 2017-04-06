using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    public class FieldGrid : IPlayerGrid
    {
        private bool[] _grid;

        public int Size { get; private set; }

        public FieldGrid(int size)
        {
            _grid = new bool[size * size];
            Size = size;
        }

        public void SetCell(int pos, bool value = true)
        {
            _grid[pos] = value;
        }

        public bool GetCell(int pos)
        {
            return _grid[pos];
        }

        public BoolGrid GetRowArray(int pos, out int maxInARow)
        {
            int row = GetRow(pos);
            bool[] rowArray = new bool[Size * Size];
            int startPos = row * Size;
            bool hasFound = false;
            int inARow = 0;
            maxInARow = inARow;
            for (int x = startPos; x < startPos + (Size - 1); x++)
            {
                if (hasFound && !_grid[x]) break; //Jump out if next cell after row of filled is empty.
                hasFound = _grid[x];
                rowArray[x] = _grid[x];
                inARow = hasFound ? inARow + 1 : 0;
                maxInARow = inARow > maxInARow ? inARow : maxInARow;
            }

            return ConvertBoolArrayToBoolGrid(rowArray);
        }

        public BoolGrid GetColumnArray(int pos, out int maxInARow)
        {
            int column = GetColumn(pos);
            bool[] columnArray = new bool[Size * Size];
            bool hasFound = false;
            int inARow = 0;
            maxInARow = inARow;
            for (int y = column; y < _grid.Length - (Size - column); y += Size)
            {
                if (hasFound && !_grid[y]) break; //Jump out if next cell after row of filled is empty.
                hasFound = _grid[y];
                columnArray[y] = _grid[y];
                inARow = hasFound ? inARow + 1 : 0;
                maxInARow = inARow > maxInARow ? inARow : maxInARow;
            }

            return ConvertBoolArrayToBoolGrid(columnArray);
        }

        /*
           Row
            0   0  1  2  3  4  5  6  7  8  9
            1  10 11 12 13 14 15 16 17 18 19
            2  20 21 22 23 24 25 26 27 28 29
            3  30 31 32 33 34 35 36 37 38 39
            4  40 41 42 43 44 45 46 47 48 49
            5  50 51 52 53 54 55 56 57 58 59
            6  60 61 62 63 64 65 66 67 68 69
            7  70 71 72 73 74 75 76 77 78 79
            8  80 81 82 83 84 85 86 87 88 89
            9  90 91 92 93 94 95 96 97 98 99

                0  1  2  3  4  5  6  7  8  9 Column
        */
        public BoolGrid GetSlashDiagArray(int pos, out int maxInARow)
        {
            bool[] diagArray = new bool[Size * Size];
            var step = Size - 1;
            var start = pos - Math.Min(Size - (GetColumn(pos) + 1), GetRow(pos)) * step; //Find min startPos for use as limit.
            int tester = pos - step; //Start one step before pos.
            if (tester < pos)
            {
                tester = pos; // start can never be less than pos.
            }
            int startPos = tester;
            while (tester >= start)
            {
                startPos = tester; //Set startPos one step before as long as cell is filled.
                if (!_grid[tester]) break; //If one step before is empty, exit.
                tester -= step;
            }
            //Console.WriteLine(" pos: {0} - startPos: {1}", pos, startPos);
            bool hasFound = false;
            int inARow = 0;
            maxInARow = inARow;
            for (int i = startPos; i < _grid.Length; i += step)
            {
                if (hasFound && !_grid[i]) break; //Jump out if next cell after row of filled is empty.
                hasFound = _grid[i];
                inARow = hasFound ? inARow + 1 : 0;
                maxInARow = inARow > maxInARow ? inARow : maxInARow;
                diagArray[i] = _grid[i];
            }

            return ConvertBoolArrayToBoolGrid(diagArray);
        }

        public BoolGrid GetBackSlashDiagArray(int pos, out int maxInARow)
        {
            bool[] diagArray = new bool[Size * Size];
            var startPos = pos - Math.Min(GetColumn(pos), GetRow(pos)) * (Size + 1);           
            bool hasFound = false;
            int inARow = 0;
            maxInARow = inARow;
            for (int i = startPos; i < _grid.Length; i += (Size + 1))
            {
                if (hasFound && !_grid[i]) break; //Jump out if next cell after row of filled is empty.
                hasFound = _grid[i];
                inARow = hasFound ? inARow + 1 : 0;
                maxInARow = inARow > maxInARow ? inARow : maxInARow;
                diagArray[i] = _grid[i];
            }

            return ConvertBoolArrayToBoolGrid(diagArray);
        }

        public int GetRow(int pos)
        {
            return pos / Size;
        }

        public int GetColumn(int pos)
        {
            return pos % Size;
        }

        public void ConvertFromIntArray(int[,] intArray)
        {
            bool[] grid = new bool[(intArray.GetUpperBound(0) + 1) ^ 2];

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    grid[y * Size + x] = intArray[x, y] != (int)Tools.Player.Empty;
                }
            }

            _grid = grid;
            Size = _grid.Length;
        }

        public BoolGrid ConvertBoolArrayToBoolGrid(bool[] grid1D)
        {
            var grid2D = GetEmptyGrid();

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    grid2D.Grid[x, y] = grid1D[y * Size + x];
                }
            }
            return grid2D;
        }

        public BoolGrid GetEmptyGrid()
        {
            return new BoolGrid(Size);
        }

        public string PrintArray(bool[] array)
        {
            string arrayString = "";
            for (int x = 0; x < array.Length; x++)
            {
                arrayString += array[x] ? 'x' : ' ';
                if ((x + 1) % Size == 0)
                {
                    arrayString += '\n';
                }             
            }           

            return arrayString;
        }
          
        public override string ToString()
        {
            string grid = "";
            for (int y = Size - 1; y >= 0; y--)
            {
                grid += string.Format("{0:D2}: ", y);
                for (int x = 0; x < Size; x++)
                {
                    grid += _grid[y * Size + x] ? "x " : ". ";
                }
                grid += '\n';
            }

            return grid;
        }
    }
}
