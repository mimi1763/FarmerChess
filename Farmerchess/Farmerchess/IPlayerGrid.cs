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
        bool[] GetRowArray(int pos);
        bool[] GetColumnArray(int pos);
        bool[] GetSlashDiagArray(int pos);
        bool[] GetBackSlashDiagArray(int pos);
        void ConvertFromIntArray(int[,] intArray);
        string PrintArray(bool[] array);
    }
}
