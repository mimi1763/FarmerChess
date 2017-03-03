using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    class GameTree
    {
        private List<BitGrid>[] _treeArray;

        public GameTree(int levels)
        {
            _treeArray = new List<BitGrid>[levels];
        }

        public List<BitGrid> GetLevel(int level)
        {
            if (level > GetLevelCount() - 1)
            {
                return null;
            }
            return _treeArray[level];
        }

        public int GetLevelCount()
        {
            return _treeArray.Length;
        }

        public int GetGridCountAtLevel(int level)
        {
            if (level > GetLevelCount() - 1)
            {
                return -1;
            }
            return _treeArray[level].Count;
        }

        public List<BitGrid>[] Tree {
            get { return _treeArray; }
            set { _treeArray = value; }
        }
    }
}
