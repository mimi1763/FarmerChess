using Farmerchess.Gui;
using Farmerchess.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    class Game
    {
        public static readonly int LEVELS = 10;

        private GameTree _gameTree;
        IPlayer[] _players;
        public Tools.Player Turn { get; private set; }

        public Game()
        {
            _gameTree = new GameTree(LEVELS);
            _players[0] = new Human(Tools.Player.X);
            _players[1] = new MiniMaxAI(Tools.Player.O, ref _gameTree);
        }

        public void Start()
        {
            Turn = Tools.Player.X;
            bool hasWon = false;

            /*************************
             ***** Main Game Loop ****
             *************************/
            //while (!hasWon)
            //{


            //    ChangeTurn();
            //}
        }

        private Tools.Player SwapPlayer(Tools.Player lastPlayer)
        {
            return lastPlayer == Tools.Player.X ? Tools.Player.O : Tools.Player.X;
        }

        public void ChangeTurn()
        {
            Turn = SwapPlayer(Turn);
        }
    }
}
