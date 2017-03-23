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

        public bool[] GetRowArray(int pos)
        {
            int row = GetRow(pos);
            bool[] rowArray = new bool[Size];
            int startPos = row * Size;
            int index = 0;
            for (int x = startPos; x < startPos + (Size - 1); x++)
            {
                rowArray[index++] = _grid[x];
            }

            return rowArray;
        }

        public bool[] GetColumnArray(int pos)
        {
            int column = GetColumn(pos);
            bool[] columnArray = new bool[Size];
            int index = 0;
            for (int y = column; y < _grid.Length - (Size - column); y += Size)
            {
                columnArray[index++] = _grid[y];
            }

            return columnArray;
        }

        /*
            0   0  1  2  3  4
            1   5  6  7  8  9
            2  10 11 12 13 14
            3  15 16 17 18 19
            4  20 21 22 23 24

               a  b  c  d  e
        */
        public bool[] GetSlashDiagArray(int pos)
        {
            int column = GetColumn(pos);
            int row = GetRow(pos);
            bool[] diagArray = new bool[Size * Size];
            int startPos = pos - (row * Size) - row;
            startPos = startPos >= 0 ? startPos : 0;
            for (int i = startPos; i < _grid.Length; i += (Size + 1))
            {
                diagArray[i] = _grid[i];
            }

            return diagArray;
        }

        public bool[] GetBackSlashDiagArray(int pos)
        {
            int column = GetColumn(pos);
            int row = GetRow(pos);
            bool[] diagArray = new bool[Size * Size];
            int startPos = pos - (row * Size) + row;
            for (int i = startPos; i < _grid.Length; i += (Size - 1))
            {
                diagArray[i] = _grid[i];
            }

            return diagArray;
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
