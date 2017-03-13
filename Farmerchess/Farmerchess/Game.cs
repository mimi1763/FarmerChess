using Farmerchess.Gui;
using Farmerchess.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Farmerchess
{
    class Game
    {
        public static readonly int LEVELS = 10;

        private GameTree _gameTree;
        IPlayer[] _players;
        Board _board;

        public GameCanvas Canvas { get { return _board.Canvas; } }

        public Size GameWindowSize { get { return _board.GetWindowSize(); } }

        public Tools.Player Turn { get; private set; }

        public Game()
        {
            _gameTree = new GameTree(LEVELS);
            _players[0] = new Human(Tools.Player.X);
            _players[1] = new MiniMaxAI(Tools.Player.O, ref _gameTree);
            InitGame();
        }

        private void InitGame()
        {
            var useTestGrid = (int)Tools.ReadSetting(Tools.SettingsKey_UseTestGrid, true);
            InitGui(useTestGrid > 0);
            InitBitGrids();
            _board.Draw();
            //_game.Start();
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

        private void InitGui(bool useTestGrid)
        {
            int blocksX;
            int blocksY;
            int blockSize = (int)Tools.ReadSetting(Tools.SettingsKey_BlockSize, true);

            if (useTestGrid)
            {
                blocksX = blocksY = BitGrid.TestGridSize;
            }
            else
            {
                blocksX = blocksY = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
            }

            _board = new Board(blocksX, blocksY, blockSize, useTestGrid);
        }

        public void Draw()
        {
            _board.Draw();
        }

        private void InitBitGrids()
        {
            _players[0].Grid = new BitGrid(Tools.Player.X, _board);
            _players[1].Grid = new BitGrid(Tools.Player.O, _board);
        }

        public void ChangeCellAtMousePos(MouseDevice mouse)
        {
            Point position = mouse.GetPosition(_board.Canvas);
            int player = Turn == Tools.Player.X ? (int)Tools.Player.X : (int)Tools.Player.O;
            var cell = _board.GetCell((int)position.X, (int)position.Y, player);
            _board.DrawCell(cell);
            ChangeTurn();
        }
    }
}
