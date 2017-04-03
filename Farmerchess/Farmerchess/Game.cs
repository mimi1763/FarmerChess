using Farmerchess.Gui;
using Farmerchess.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Farmerchess
{
    class Game
    {
        public static readonly int LEVELS = 4;

        List<IPlayer> _players;
        Board _board;
        private bool _isDebug;
        DebugWindow _debugWindow;

        public List<GameCanvas> CanvasList { get; private set; }

        public Size GameWindowSize { get { return _board.GetWindowSize(); } }

        public Tools.Player Turn { get; private set; }

        public Game()
        {
            _players = new List<IPlayer>();
            _players.Add(new Human(Tools.Player.X));
            _players.Add(new MiniMaxAI(Tools.Player.O));           
            InitGame();
            _isDebug = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_DebugMode) == 1;
            if (_isDebug)
            {
                InitDebugWindow();
            }
        }

        private void InitGame()
        {
            var useTestGrid = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_UseTestGrid);
            InitGui(useTestGrid > 0);
            InitPlayerGrids(useTestGrid > 0);
            CanvasList = new List<GameCanvas>();
            CanvasList.Add(_board.Canvas);
            _board.Draw();
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
            int blockSize = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_BlockSize);

            if (useTestGrid)
            {
                blocksX = blocksY = BitGrid.TestGridSize;
            }
            else
            {
                blocksX = blocksY = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_BlockCountX);
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
                _players[player - 1].Grid.SetCell(cell.Id, true);
                _board.DrawCell(cell);

                if (_isDebug)
                {
                    ChangeDebugWindowCell(cell.GridX, cell.GridY);

                    //LogToDebugWindow(cell.Id.ToString());
                    //LogToDebugWindow(_players[player - 1].Grid.ToString());
                    //var array = _players[player - 1].Grid.GetSlashDiagArray(cell.Id);
                    //LogToDebugWindow(_players[player - 1].Grid.PrintArray(array));
                }

                ChangeTurn();
            }
        }

        private void InitDebugWindow()
        {
            _debugWindow = new DebugWindow(_board);
            _debugWindow.Width = 600;
            _debugWindow.Height = 600;
            _debugWindow.Show();
        }

        public void LogToDebugWindow(string text)
        {
            _debugWindow.AddString(text);            
        }

        public void ChangeDebugWindowCell(int posx, int posy)
        {
            _debugWindow.ChangeBoardCell(posx, posy);
        }

        class DebugWindow : Window
        {
            StackPanel _panel;
            Board _debugBoard;

            public DebugWindow()
            {
                ScrollViewer scroller = new ScrollViewer();
                scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                _panel = new StackPanel();
                scroller.Content = _panel;
                this.Content = scroller;
            }

            public DebugWindow(Board board)
            {
                _debugBoard = new Board(board);
                this.Content = _debugBoard;
            }

            public void AddString(string text)
            {
                var label = new Label();
                label.Content = text;
                _panel.Children.Add(label);
            }

            public void ChangeBoardCell(int posx, int posy)
            {
                Cell cell = _debugBoard.GetCell(posx, posy, 0);
                _debugBoard.DrawCell(cell);
            }
        }
    }
}
