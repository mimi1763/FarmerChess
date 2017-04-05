using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess.Players
{
    class Human : IPlayer
    {
        public Human (Tools.Player colour)
        {
            PlayerKind = colour;
        }
        public Tools.Player PlayerKind
        {
            get; private set;
        }

        public IPlayerGrid Grid
        {
            get; set;
        }

        public void Move()
        {
        }
    }
}
