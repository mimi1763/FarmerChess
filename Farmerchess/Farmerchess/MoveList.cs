using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    class MoveList
    {
        private List<Move> _moveList;

        public MoveList()
        {
            _moveList = new List<Move>();
        }

        public int Count
        {
            get { return _moveList.Count; }
        }

        /// <summary>
        /// Add move to list.
        /// </summary>
        /// <param name="move"></param>
        public void AddMove(Move move)
        {
            _moveList.Add(move);
        }

        /// <summary>
        /// Retrieve move from list at specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Move GetMove(int index)
        {
            if (index < _moveList.Count)
            {
                return _moveList[index];
            }

            return null;
        }

        /// <summary>
        /// Pop last move, removing it from list before returning it.
        /// </summary>
        /// <returns></returns>
        public Move PopMove()
        {
            if (_moveList.Count > 0)
            {
                var move = _moveList.Last();
                _moveList.RemoveAt(_moveList.Count - 1);
                return move;
            }

            return null;
        }

        public void ClearMoveList()
        {
            _moveList.Clear();
        }
    }
}
