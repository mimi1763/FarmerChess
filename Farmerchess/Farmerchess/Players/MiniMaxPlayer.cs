using Farmerchess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess.Players
{
    class MiniMaxPlayer : IPlayer
    {
        private GameTree _gameTree;

        public MiniMaxPlayer(ref GameTree gameTree)
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
