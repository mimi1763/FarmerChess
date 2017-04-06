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

        public List<Board> BoardList { get; private set; }

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

            DrawBoards();
        }

        private void InitGame()
        {
            var useTestGrid = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_UseTestGrid);
            InitGui(useTestGrid > 0);
            InitPlayerGrids(useTestGrid > 0);
            BoardList = new List<Board>();
            BoardList.Add(_board);
        }

        private void DrawBoards()
        {
            foreach (var board in BoardList)
            {
                board.Draw();
            }
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
                _players[0].Grid.ConvertFromIntArray(_players[0].PlayerKind == Tools.Player.O ? Tools.Instance.TestGridO : Tools.Instance.TestGridX);
                _players[1].Grid.ConvertFromIntArray(_players[1].PlayerKind == Tools.Player.X ? Tools.Instance.TestGridX : Tools.Instance.TestGridO);
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
                    //Draw rows connected to current cell.
                    int maxInARow = 0;
                    var array = _players[player - 1].Grid.GetSlashDiagArray(cell.Id, out maxInARow);
                    Console.WriteLine("max in a row: " + maxInARow);
                    DrawGridInDebugWindow(array);

                    //Draw current cell only.
                    //ChangeDebugWindowCell(cell.Id);

                    //Text debug:
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
            _debugWindow.Width = _board.Width;
            _debugWindow.Height = _board.Height;
            _debugWindow.DrawBoard();
            _debugWindow.Show();

        }

        public void LogToDebugWindow(string text)
        {
            _debugWindow.AddString(text);            
        }

        public void DrawGridInDebugWindow(BoolGrid grid)
        {
            _debugWindow.DrawBoard(grid);
        }

        public void ChangeDebugWindowCell(int id)
        {
            _debugWindow.ChangeBoardCell(id);
        }

        public void CloseDebugWindow()
        {
            if (_debugWindow.IsLoaded)
            {
                _debugWindow.Close();
            }
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
                Init(scroller);
            }

            public DebugWindow(Board board)
            {
                _debugBoard = new Board(board);
                Init(_debugBoard.Canvas);
            }

            public Size WindowSize { get { return _debugBoard.GetWindowSize(); } }

            private void Init(UIElement content)
            {
                this.Content = content;
                this.Title = "Debug Window";
            }

            public void AddString(string text)
            {
                var label = new Label();
                label.Content = text;
                _panel.Children.Add(label);
            }

            public void ChangeBoardCell(int id)
            {
                Cell cell = _debugBoard.GetCell(id, 1);
                _debugBoard.DrawCell(cell, true);
            }

            public void DrawBoard(BoolGrid grid = null)
            {
                _debugBoard.DrawIndeces();
                _debugBoard.Draw(grid);
            }
        }
    }
}
