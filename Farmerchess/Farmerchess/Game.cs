using Farmerchess.Gui;
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

        public Game()
        {
            _gameTree = new GameTree(LEVELS);
        }

        public void Start()
        {
            var playerTurn = Board.Player.X;
            bool hasWon = false;

            /*************************
             ***** Main Game Loop ****
             *************************/
            while (!hasWon)
            {


                playerTurn = SwapPlayer(playerTurn);
            }
        }

        private Board.Player SwapPlayer(Board.Player lastPlayer)
        {
            return lastPlayer == Board.Player.X ? Board.Player.O : Board.Player.X;
        }
    }
}
