using Farmerchess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess.Players
{
    class MiniMaxAI : IPlayer
    {
        private GameTree _gameTree;

        public Tools.Player Colour
        {
            get; private set;
        }

        public BitGrid Grid
        {
            get; set;
        }

        public MiniMaxAI(Tools.Player colour, ref GameTree gameTree)
        {
            _gameTree = gameTree;
        }

        public void Move()
        {

        }

        public Node<BitGrid> AlphaBetaMiniMax(Node<BitGrid> n)
        {

            return null;
        }
    }
}
