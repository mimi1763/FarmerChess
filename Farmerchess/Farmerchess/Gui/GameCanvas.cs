using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Farmerchess.Gui
{
    class GameCanvas : Canvas
    {
        public GameCanvas(int dimX, int dimY)
        {
            this.Children.Capacity = dimX * dimY;

            for (var y = 0; y < dimY; y++)
            {
                for (var x = 0; x < dimX; x++)
                {
                    var path = new Path();
                    path.Tag = y * dimY + x;
                    this.Children.Add(path);
                }
            }
        }

        public void AddChild(UIElement child)
        {
            this.Children.Add(child);
        }
    }
}
