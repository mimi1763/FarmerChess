using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Farmerchess.Gui
{
    class GfxElement
    {
        private BitmapImage _bitmapImage;
        private Image _image;

        public Image Image { get { return _image; } }

        public GfxElement(string imagePath)
        {
            try
            {
                _bitmapImage = new BitmapImage(new Uri(imagePath));
                _image = new Image();
                _image.Source = _bitmapImage;
                _image.Width = _bitmapImage.Width;
                _image.Height = _bitmapImage.Height;
            }
            catch (Exception) {}
        }

        public bool IsValid()
        {
            return _bitmapImage != null && _image != null;
        }

        public void AddToCanvas(GameCanvas canvas)
        {
            //Canvas.SetLeft(_image, rnd.NextDouble() * maxX);
            //Canvas.SetTop(_image, rnd.NextDouble() * maxY);
            canvas.AddChild(_image);
        }
    }
}
