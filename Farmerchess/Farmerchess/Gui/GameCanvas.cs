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
    class GameCanvas : Canvas
    {
        private Canvas _gridCanvas;
         
        public GameCanvas(int dimX, int dimY, int blockSize)
        {
            _gridCanvas = new Canvas();
            _gridCanvas.Background = CreateGridBackground();
            this.Children.Add(_gridCanvas);

            for (var y = 0; y < dimY; y++)
            {
                for (var x = 0; x < dimX; x++)
                {
                    var rectangle = new Rectangle();
                    rectangle.Width = rectangle.Height = blockSize;
                    rectangle.RenderTransform = new TranslateTransform(x * blockSize, y * blockSize);
                    _gridCanvas.Children.Add(rectangle);
                }
            }
        }

        public Canvas GridCanvas
        {
            get { return _gridCanvas; }
        } 

        public void SetGridSize(int width, int height)
        {
            _gridCanvas.Width = width;
            _gridCanvas.Height = height;
        }

        public UIElement GetGridChild(int index)
        {
            return _gridCanvas.Children[index];
        }

        public void AddGridChild(UIElement child)
        {
            _gridCanvas.Children.Add(child);
        }

        public DrawingBrush CreateGridBackground()
        {
            var drawingBrush = new DrawingBrush();
      
            var rect = new Rect(0, 0, 40, 40);
            var geoDrawing = new GeometryDrawing();
            var rectGeo = new RectangleGeometry();
            var pen = new Pen();
            rectGeo.Rect = rect;
            geoDrawing.Geometry = rectGeo;
            pen.Brush = Brushes.Black;
            pen.Thickness = 2;
            geoDrawing.Pen = pen;
            drawingBrush.TileMode = TileMode.Tile;
            drawingBrush.Viewport = rect;
            drawingBrush.ViewportUnits = BrushMappingMode.Absolute;
            drawingBrush.Drawing = geoDrawing;

            return drawingBrush;
        }

        public void Complete()
        {
            MeasureAndArrangeCanvas(_gridCanvas);
            MeasureAndArrangeCanvas(this);
        }

        private void MeasureAndArrangeCanvas(Canvas canvas)
        {
            var size = new Size(canvas.Width, canvas.Height);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));
        }
    }
}
