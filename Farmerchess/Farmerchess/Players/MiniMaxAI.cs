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
        public Tools.Player Colour
        {
            get; private set;
        }

        public IPlayerGrid Grid
        {
            get; set;
        }

        public MiniMaxAI(Tools.Player colour)
        {
        }

        public void Move()
        {

        }

        public Node<FieldGrid> AlphaBetaMiniMax(Node<FieldGrid> n)
        {

            return null;
        }
    }
}
