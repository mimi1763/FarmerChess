using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Farmerchess.Gui
{
    class Board
    {
        private GameCanvas _canvas;
        private Cell[,] _cellGrid;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int BlockCountX { get; private set; }
        public int BlockCountY { get; private set; }
        public int BlockSize { get; private set; }

        public GameCanvas Canvas
        {
            get { return _canvas; }
        }

        public Board(int blockCountX, int blockCountY, int blockSize, bool useBitGrid = false)
        {
            BlockCountX = blockCountX;
            BlockCountY = blockCountY;
            BlockSize = blockSize;
            InitGui();
            InitCellGrid();
        }

        //Copy constructor
        public Board(Board boardToCopy)
        {
            BlockCountX = boardToCopy.BlockCountX;
            BlockCountY = boardToCopy.BlockCountY;
            BlockSize = boardToCopy.BlockSize;
            InitGui();
            InitCellGrid();
        }

        private void InitGui()
        {
            Width = BlockCountX * BlockSize;
            Height = BlockCountY * BlockSize;
            _canvas = new GameCanvas(BlockCountX, BlockCountY, BlockSize);
            _canvas.SetGridSize(Width, Height);
            _canvas.Width = Width * 1.5;
            _canvas.Height = Height * 1.5;
            //_canvas.Complete();
        }

        private void InitCellGrid(BoolGrid grid = null)
        {
            _cellGrid = new Cell[BlockCountX, BlockCountY];
            int id = 0;
            var pE = Tools.Player.Empty;
            var pX = Tools.Player.X;
            var pO = Tools.Player.O;
            var piece = pE;
            var lastIndex = BlockCountX * BlockCountX - 1;
            for (var y = 0; y < BlockCountY; y++)
            {
                for (var x = 0; x < BlockCountX; x++)
                {
                    id = y * BlockCountY + x;
                    if (grid != null)
                    {
                        piece = grid.Grid[x, y] ? pX : pE;
                    }
                    _cellGrid[x, y] = new Cell(x * BlockSize, y * BlockSize, (int)piece, id);
                    _cellGrid[x, y].Rectangle = new Rect(_cellGrid[x, y].PosX, _cellGrid[x, y].PosY, BlockSize, BlockSize);
                    _cellGrid[x, y].RectGeo = new RectangleGeometry(_cellGrid[x, y].Rectangle);
                }
            }
        }

        private void ClearGrid(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle;
            for (int y = 0; y < BlockCountY; y++)
            {
                for (int x = 0; x < BlockCountX; x++)
                {
                    _cellGrid[x, y].Value = (int)Tools.Player.Empty;
                    rectangle = (Rectangle)Canvas.GetGridChild(y * BlockCountY + x);
                    if (rectangle != null)
                    {
                        rectangle.Fill = Brushes.Transparent;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the size for main window dimensions.
        /// </summary>
        /// <returns></returns>
        public Size GetWindowSize()
        {
            return new Size(Width + BlockSize, Height + 2 * BlockSize);
        }

        /// <summary>
        /// Draw entire board 
        /// </summary>
        public void Draw(BoolGrid grid = null)
        {
            if (grid != null)
            {
                InitCellGrid(grid);
            }
            for (int y = 0; y < BlockCountY; y++)
            {
                for (int x = 0; x < BlockCountX; x++)
                {
                    DrawCell(_cellGrid[x, y]);
                }
            }
        }

        public void DrawIndeces()
        {
            for (int y = 0; y < BlockCountY; y++)
            {
                for (int x = 0; x < BlockCountX; x++)
                {
                    DrawCell(_cellGrid[x, y], false, true);
                }
            }
        }

        public void DrawCell(Cell cell, bool isDebug = false)
        {
            DrawGridCell(cell, isDebug, false);
        }

        public void DrawCell(Cell cell, bool isDebug, bool drawIndex)
        {
            DrawGridCell(cell, isDebug, drawIndex);
        }

        /// <summary>
        /// Draw single cell
        /// </summary>
        /// <param name="cell"></param>
        private void DrawGridCell(Cell cell, bool isDebug, bool drawIndex)
        {
            var value = cell != null ? cell.Value : 0;
            var id = 0;
            if (cell != null)
            {
                id = cell.Id < Canvas.GridCanvas.Children.Count ? cell.Id : Canvas.GridCanvas.Children.Count - 1;
            }
            var imgRect = (Rectangle)Canvas.GetGridChild(id);
            if (cell != null)
            {
                if (drawIndex)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Foreground = Brushes.Black;
                    textBlock.Text = cell.Id.ToString();
                    VisualBrush visBrush = new VisualBrush();
                    visBrush.Visual = textBlock;
                    imgRect.Fill = visBrush;
                }
                else if (value > 0)
                {
                    if (!isDebug)
                    {
                        imgRect.Fill = Tools.Instance.GetImageBrush(value);
                    } 
                    else
                    {
                        imgRect.Fill = Brushes.Teal;
                    }               
                }
                else
                {
                    imgRect.Fill = Brushes.Transparent;
                }
            }
        }

        /// <summary>
        /// Gets gui board cell at mouse position.
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Cell GetCell(int posx, int posy, int value = -1)
        {
            int sx = posx / BlockSize;
            int sy = posy / BlockSize;

            for (var y = sy; y < BlockCountY; y++)
            {
                for (var x = sx; x < BlockCountX; x++)
                {
                    if (posx > _cellGrid[x, y].PosX && posx < _cellGrid[x, y].PosX + BlockSize &&
                        posy > _cellGrid[x, y].PosY && posy < _cellGrid[x, y].PosY + BlockSize)
                    {
                        return GetCellAtGridPos(x, y, value);
                    }
                }
            }
            return null;
        }

        public Cell GetCell(int id, int value = -1)
        {
            int x = id % BlockCountX;
            int y = id / BlockCountY;
            return GetCellAtGridPos(x, y, value);
        }

        private Cell GetCellAtGridPos(int x, int y, int value)
        {
            if (value > -1)
            {
                _cellGrid[x, y].Value = Math.Abs(value - _cellGrid[x, y].Value);
            }
            _cellGrid[x, y].GridX = x;
            _cellGrid[x, y].GridY = y;

            return _cellGrid[x, y];
        }
    }
}
