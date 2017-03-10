using Farmerchess.Gui;
using System.Windows;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Numerics;

namespace Farmerchess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board _board;
        BitGrid[] _bitGrids;
        Game _game;

        public MainWindow()
        {
            InitializeComponent();
            var useTestGrid = (int)Tools.ReadSetting(Tools.SettingsKey_UseTestGrid, true);
            InitGui(useTestGrid > 0);
            InitGame();
            InitGrids(useTestGrid > 0);
            _board.Draw();
        }

        private void InitGame()
        {           
            _game = new Game();
            //_game.Start();
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
            var size = _board.GetWindowSize();
            this.Width = size.Width;
            this.Height = size.Height;
            this.Content = _board.Canvas;
        }

        private void InitGrids()
        {
            _bitGrids[0] = new BitGrid(Tools.Player.X, _board);
            _bitGrids[1] = new BitGrid(Tools.Player.O, _board);

            _grid = new Cell[_blockCountX, _blockCountY];
            int id = 0;
            var piece = Tools.Player.Empty;
            for (var y = 0; y < _blockCountY; y++)
            {
                for (var x = 0; x < _blockCountX; x++)
                {
                    id = y * _blockCountX + x;
                    piece = _bitGrids[0].
                    _grid[x, y] = new Cell(x * _blockSize, y * _blockSize, (int)piece, id);
                    _grid[x, y].Rectangle = new Rect(_grid[x, y].PosX, _grid[x, y].PosY, _blockSize, _blockSize);
                    _grid[x, y].RectGeo = new RectangleGeometry();
                    _grid[x, y].RectGeo.Rect = _grid[x, y].Rectangle;
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCellAtMousePos(e);
        }

        private void ChangeCellAtMousePos(MouseButtonEventArgs e)
        {
            //CompareBigIntegerToLong();

            var position = e.MouseDevice.GetPosition(_board.Canvas);
            int player = _game.Turn == Tools.Player.X ? (int)Tools.Player.X : (int)Tools.Player.O;
            var cell = _board.GetCell((int)position.X, (int)position.Y, player);
            _board.DrawCell(cell);
            _game.ChangeTurn();
        }

        private void CompareBigIntegerToLong()
        {
            int bitSize = 40 * 40;
            Stopwatch watch = new Stopwatch();
            BigInteger bigInt = new BigInteger(bitSize);
            long int64 = 1;
            bigInt = 1;

            watch.Start();
            for (var i = 0; i < 64; i++)
                int64 = int64 << 1;
            watch.Stop();
            Console.WriteLine("long took: {0} ms to complete.", watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            for (var i = 0; i < bitSize; i++)
                bigInt = bigInt << 1;
            watch.Stop();
            Console.WriteLine("BigInteger took: {0} ms to complete.", watch.ElapsedMilliseconds);
        }
    }
}
