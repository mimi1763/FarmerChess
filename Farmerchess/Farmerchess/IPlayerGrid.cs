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
        BoolGrid GetRowArray(int pos);
        BoolGrid GetColumnArray(int pos);
        BoolGrid GetSlashDiagArray(int pos);
        BoolGrid GetBackSlashDiagArray(int pos);
        void ConvertFromIntArray(int[,] intArray);
        string PrintArray(bool[] array);
    }
}
