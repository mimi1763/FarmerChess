using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Farmerchess.Gui
{
    class Board
    {
        private Canvas _canvas;
        private static int _blockCountX;
        private static int _blockCountY;
        private static int _blockSize;
        private static int _lineThickness;
        private Cell[,] _grid;
        private SolidColorBrush _bgColour;
        private SolidColorBrush _gridColour;
        private SolidColorBrush _oColour;
        private SolidColorBrush _xColour;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Canvas Canvas
        {
            get { return _canvas; }
            set { _canvas = value; }
        }

        public Board(int blockCountX, int blockCountY, int blockSize)
        {
            _blockCountX = blockCountX;
            _blockCountY = blockCountY;
            _blockSize = blockSize;
            InitGui();
            InitGrid();
        }

        private void InitGui()
        {
            Width = _blockCountX * _blockSize;
            Height = _blockCountY * _blockSize;
            _bgColour = Tools.GetBrush(Tools.BgColour);
            _gridColour = Tools.GetBrush(Tools.GridColour);
            _oColour = Tools.GetBrush(Tools.OColour);
            _xColour = Tools.GetBrush(Tools.XColour);
            Canvas = new Canvas();
            Canvas.Width = Width;
            Canvas.Height = Height;
            Canvas.Background = _bgColour;
            int lineThickness;
            bool success = int.TryParse(Tools.ReadSetting(Tools.LineThickness), out lineThickness);
            _lineThickness = success ? lineThickness : 1;
        }

        private void InitGrid()
        {
            _grid = new Cell[_blockCountX, _blockCountY];
            for (var y = 0; y < _blockCountY; y++)
            {
                for (var x = 0; x < _blockCountX; x++)
                {
                    _grid[x, y] = new Cell(x * _blockSize, y * _blockSize, 0);
                }
            }
        }

        /// <summary>
        /// Returns the size for main window dimensions.
        /// </summary>
        /// <returns></returns>
        public Size GetWindowSize()
        {
            return new Size(Width + _blockSize, Height + 2 * _blockSize);
        }
        public void Draw()
        {
            int x, y;

            //Clear the canvas.
            Canvas.Children.Clear();

            for (y = 0; y < _blockCountY; y++)
            {
                for (x = 0; x < _blockCountX; x++)
                {
                    var cell = _grid[x, y];
                    var value = cell != null ? cell.Value : 0;
                    if (cell != null)
                    {
                        var rect = new Path
                        {
                            Data = new RectangleGeometry(new Rect(cell.PosX, cell.PosY, _blockSize, _blockSize)),
                            Stroke = _gridColour,
                            StrokeThickness = _lineThickness,
                            Fill = value > 0 ? _oColour : Brushes.Transparent
                        };
                        Canvas.Children.Add(rect);
                    }
                }
            }
        }

        public Cell GetCell(int posx, int posy, int value = -1)
        {
            for (var y = 0; y < _blockCountY; y++)
            {
                for (var x = 0; x < _blockCountX; x++)
                {
                    if (posx > _grid[x, y].PosX && posx < _grid[x, y].PosX + _blockSize &&
                        posy > _grid[x, y].PosY && posy < _grid[x, y].PosY + _blockSize)
                    {
                        if (value > -1)
                        {
                            _grid[x, y].Value = Math.Abs(value - _grid[x, y].Value);
                        }
                        return _grid[x, y];
                    }
                }
            }
            return null;
        }
    }
}
