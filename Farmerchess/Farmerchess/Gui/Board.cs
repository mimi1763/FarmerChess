using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Farmerchess.Gui
{
    class Board
    {
        private GameCanvas _canvas;
        private static int _blockSize;
        private static int _lineThickness;
        private Cell[,] _cellGrid;
        private SolidColorBrush _bgColour;
        private SolidColorBrush _gridColour;
        private SolidColorBrush _oColour;
        private SolidColorBrush _xColour;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int BlockCountX { get; private set; }
        public int BlockCountY { get; private set; }

        public GameCanvas Canvas
        {
            get { return _canvas; }
            //set { _canvas = value; }
        }

        public Board(int blockCountX, int blockCountY, int blockSize, bool useBitGrid = false)
        {
            BlockCountX = blockCountX;
            BlockCountY = blockCountY;
            _blockSize = blockSize;
            InitGui();
            InitCellGrid();
        }

        private void InitGui()
        {
            Width = BlockCountX * _blockSize;
            Height = BlockCountY * _blockSize;
            _bgColour = Tools.GetBrush(Tools.Instance.SettingsKey_BgColour);
            _gridColour = Tools.GetBrush(Tools.Instance.SettingsKey_GridColour);
            _oColour = Tools.GetBrush(Tools.Instance.SettingsKey_OColour);
            _xColour = Tools.GetBrush(Tools.Instance.SettingsKey_XColour);
            _canvas = new GameCanvas(BlockCountX, BlockCountY, _blockSize);
            _canvas.SetGridSize(Width, Height);
            _canvas.Width = Width * 1.5;
            _canvas.Height = Height * 1.5;
            _canvas.Complete();
            //_canvas.GridCanvas.Background = _bgColour;
            int thickness = (int)Tools.ReadSetting(Tools.Instance.SettingsKey_LineThickness, true);
            _lineThickness = thickness < 0 ? 1 : thickness;
        }

        private void InitCellGrid()
        {
            _cellGrid = new Cell[BlockCountX, BlockCountY];
            int id = 0;
            var piece = Tools.Player.Empty;
            for (var y = 0; y < BlockCountY; y++)
            {
                for (var x = 0; x < BlockCountX; x++)
                {
                    id = y * BlockCountX + x;
                    _cellGrid[x, y] = new Cell(x * _blockSize, y * _blockSize, (int)piece, id);
                    _cellGrid[x, y].ImgRectangle = new Rectangle();
                    _cellGrid[x, y].Rectangle = new Rect(_cellGrid[x, y].PosX, _cellGrid[x, y].PosY, _blockSize, _blockSize);
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
            return new Size(Width + _blockSize, Height + 2 * _blockSize);
        }

        /// <summary>
        /// Draw entire board 
        /// </summary>
        public void Draw()
        {
            for (int y = 0; y < BlockCountY; y++)
            {
                for (int x = 0; x < BlockCountX; x++)
                {
                    DrawCell(_cellGrid[x, y]);
                }
            }
        }

        /// <summary>
        /// Draw single cell
        /// </summary>
        /// <param name="cell"></param>
        public void DrawCell(Cell cell)
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
                if (value > 0)
                {
                    imgRect.Fill = Tools.GetImageBrush(value);
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
            for (var y = 0; y < BlockCountY; y++)
            {
                for (var x = 0; x < BlockCountX; x++)
                {
                    if (posx > _cellGrid[x, y].PosX && posx < _cellGrid[x, y].PosX + _blockSize &&
                        posy > _cellGrid[x, y].PosY && posy < _cellGrid[x, y].PosY + _blockSize)
                    {
                        if (value > -1)
                        {
                            _cellGrid[x, y].Value = Math.Abs(value - _cellGrid[x, y].Value);
                        }
                        return _cellGrid[x, y];
                    }
                }
            }
            return null;
        }
    }
}
