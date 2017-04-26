using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    interface IPlayerGrid
    {
        void SetCell(int pos, bool value);
        bool GetCell(int pos);
        int GetMaxRowDirection(int pos, out int maxInARow);
        BoolGrid GetAllArrays(int pos, out int maxInARow, out bool isOpen);
        BoolGrid GetRowArray(int pos, out int maxInARow, out bool isOpen);
        BoolGrid GetColumnArray(int pos, out int maxInARow, out bool isOpen);
        BoolGrid GetSlashDiagArray(int pos, out int maxInARow, out bool isOpen);
        BoolGrid GetBackSlashDiagArray(int pos, out int maxInARow, out bool isOpen);
        void ConvertFromIntArray(int[,] intArray);
        string PrintArray(bool[] array);
    }
}
