using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess.Players
{
    interface IPlayer
    {
        Tools.Player Colour { get; }
        IPlayerGrid Grid { get; set; }
        void Move();
    }
}
