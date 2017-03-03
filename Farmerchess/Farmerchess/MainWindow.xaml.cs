using Farmerchess.Gui;
using System.Windows;
using System.Windows.Input;
using System;

namespace Farmerchess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board _board;
        Game _game;

        public MainWindow()
        {
            InitializeComponent();
            var useTestGrid = (int)Tools.ReadSetting(Tools.SettingsKey_UseTestGrid, true);
            InitGui(useTestGrid > 0);
            InitGame();
            _board.Draw();
        }

        private void InitGame()
        {           
            _game = new Game();
        }

        private void InitGui(bool useTestGrid)
        {
            int blocksX;
            int blocksY;
            int blockSize = (int)Tools.ReadSetting(Tools.SettingsKey_BlockSize, true);
            
            if (useTestGrid)
            {
                var testGridSize = BitGrid.TestGridSize;
                blocksX = (int)testGridSize.Width;
                blocksY = (int)testGridSize.Height;
            }
            else
            {
                blocksX = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
                blocksY = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountY, true);
            }       

            _board = new Board(blocksX, blocksY, blockSize, useTestGrid);
            var size = _board.GetWindowSize();
            this.Width = size.Width;
            this.Height = size.Height;
            this.Content = _board.Canvas;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCellAtMousePos(e);
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCellAtMousePos(e, true);
        }

        private void ChangeCellAtMousePos(MouseButtonEventArgs e, bool isRightButton = false)
        {
            var position = e.MouseDevice.GetPosition(_board.Canvas);
            int player = isRightButton ? (int)Board.Player.X : (int)Board.Player.O;
            var cell = _board.GetCell((int)position.X, (int)position.Y, player);
            _board.DrawCell(cell);
        }
    }
}
