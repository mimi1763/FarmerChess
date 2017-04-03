using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace Farmerchess.Gui
{
    class GameCanvas : Canvas
    {
        private Canvas _gridCanvas;
         
        public GameCanvas(int dimX, int dimY, int blockSize)
        {
            _gridCanvas = new Canvas();
            _gridCanvas.Background = CreateGridBackground(dimX, dimY, blockSize);
            //Canvas.SetLeft(_gridCanvas, 50);
            //Canvas.SetTop(_gridCanvas, 50);
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

        //Copy constructor
        public GameCanvas(GameCanvas canvasToCopy, bool skipChildren = false)
        {
            _gridCanvas = new Canvas();
            _gridCanvas.Width = canvasToCopy.Width;
            _gridCanvas.Height = canvasToCopy.Height;
            _gridCanvas.Background = canvasToCopy._gridCanvas.Background.Clone();

            if (!skipChildren)
            {
                foreach (var child in canvasToCopy._gridCanvas.Children)
                {
                    object element;
                    string childXaml = XamlWriter.Save(child);
                    StringReader stringReader = new StringReader(childXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    if (child is Rectangle)
                    {
                        element = (Rectangle)XamlReader.Load(xmlReader);
                        _gridCanvas.Children.Add((Rectangle)element);
                    }
                    else
                    {
                        continue; //Skip child if it's not Rectangle
                    }
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

        private DrawingBrush CreateGridBackground(int dimX, int dimY, int blockSize)
        {
            var drawingBrush = new DrawingBrush();    
            var rect = new Rect(0, 0, blockSize, blockSize);
            var geoDrawing = new GeometryDrawing();
            var geoDrawingBg = new GeometryDrawing();
            var rectGeo = new RectangleGeometry();
            var drawingGrp = new DrawingGroup();
            var pen = new Pen();
            rectGeo.Rect = rect;
            geoDrawing.Geometry = geoDrawingBg.Geometry = rectGeo;
            pen.Brush = Tools.Instance.GetBrush(Tools.Instance.SettingsKey_GridColour);
            pen.Thickness = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_LineThickness);
            geoDrawing.Pen = pen;
            geoDrawingBg.Brush = Tools.Instance.GetBrush(Tools.Instance.SettingsKey_BgColour);
            drawingBrush.TileMode = TileMode.Tile;
            drawingBrush.Viewport = rect;
            drawingBrush.ViewportUnits = BrushMappingMode.Absolute;
            //Add drawing geos to drawing group
            drawingGrp.Children.Add(geoDrawingBg);
            drawingGrp.Children.Add(geoDrawing);
            //Set drawing brush's geometry as the drawing group
            drawingBrush.Drawing = drawingGrp;
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
