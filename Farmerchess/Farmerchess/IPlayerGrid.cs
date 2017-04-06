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
        BoolGrid GetRowArray(int pos, out int maxInARow);
        BoolGrid GetColumnArray(int pos, out int maxInARow);
        BoolGrid GetSlashDiagArray(int pos, out int maxInARow);
        BoolGrid GetBackSlashDiagArray(int pos, out int maxInARow);
        void ConvertFromIntArray(int[,] intArray);
        string PrintArray(bool[] array);
    }
}
