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
        public static readonly int LEVELS = 4;

        List<IPlayer> _players;
        Board _board;

        public GameCanvas Canvas { get { return _board.Canvas; } }

        public Size GameWindowSize { get { return _board.GetWindowSize(); } }

        public Tools.Player Turn { get; private set; }

        public Game()
        {
            _players = new List<IPlayer>();
            _players.Add(new Human(Tools.Player.X));
            _players.Add(new MiniMaxAI(Tools.Player.O));           
            InitGame();
        }

        private void InitGame()
        {
            var useTestGrid = (int)Tools.ReadSetting(Tools.Instance.SettingsKey_UseTestGrid, true);
            InitGui(useTestGrid > 0);
            InitPlayerGrids(useTestGrid > 0);
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
            int blockSize = (int)Tools.ReadSetting(Tools.Instance.SettingsKey_BlockSize, true);

            if (useTestGrid)
            {
                blocksX = blocksY = BitGrid.TestGridSize;
            }
            else
            {
                blocksX = blocksY = (int)Tools.ReadSetting(Tools.Instance.SettingsKey_BlockCountX, true);
            }

            _board = new Board(blocksX, blocksY, blockSize, useTestGrid);
        }

        public void Draw()
        {
            _board.Draw();
        }

        private void InitPlayerGrids(bool useTestGrid)
        {
            _players[0].Grid = new FieldGrid(_board.BlockCountX);
            _players[1].Grid = new FieldGrid(_board.BlockCountX);

            if (useTestGrid)
            {
                _players[0].Grid.ConvertFromIntArray(_players[0].Colour == Tools.Player.O ? Tools.Instance.TestGridO : Tools.Instance.TestGridX);
                _players[1].Grid.ConvertFromIntArray(_players[1].Colour == Tools.Player.X ? Tools.Instance.TestGridX : Tools.Instance.TestGridO);
            }
        }

        public void ChangeCellAtMousePos(MouseDevice mouse)
        {
            Point position = mouse.GetPosition(_board.Canvas);
            int player = Turn == Tools.Player.X ? (int)Tools.Player.X : (int)Tools.Player.O;
            var cell = _board.GetCell((int)position.X, (int)position.Y, player);
            if (cell != null)
            {
                //Debug
                Console.WriteLine(cell.Id);
                _players[player - 1].Grid.SetCell(cell.Id, true);
                _board.DrawCell(cell);
                //Debug
                Console.WriteLine(_players[player - 1].Grid.ToString());
                var array = _players[player - 1].Grid.GetSlashDiagArray(cell.Id);
                Console.WriteLine(_players[player - 1].Grid.PrintArray(array));
                ChangeTurn();
            }
        }
    }
}
