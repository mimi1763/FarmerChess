using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Farmerchess.Gui
{
    class Cell
    {
        public Cell(int posx, int posy, int value, int id)
        {
            PosX = posx;
            PosY = posy;
            Value = value;
            Id = id;
        }

        public Cell(Cell cell)
        {
            PosX = cell.PosX;
            PosY = cell.PosY;
            Value = cell.Value;
            Id = cell.Id;
            GridX = cell.GridX;
            GridY = cell.GridY;
            RectGeo = cell.RectGeo.Clone();
            var recta = new Rect();
            recta.X = cell.Rectangle.X;
            recta.Y = cell.Rectangle.Y;
            recta.Width = cell.Rectangle.Width;
            recta.Height = cell.Rectangle.Height;
            Rectangle = recta;
            //var imgRect = new Rectangle();

        }

        public int Value { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Id { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }

        public RectangleGeometry RectGeo { get; set; }
        public Rect Rectangle { get; set; }

        //public Rectangle ImgRectangle { get; set; }
    }
}
