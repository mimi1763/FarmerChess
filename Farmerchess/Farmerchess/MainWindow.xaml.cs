using Farmerchess.Gui;
using System.Windows;
using System.Windows.Input;

namespace Farmerchess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board _board;

        public MainWindow()
        {
            InitializeComponent();
            InitGui();
            _board.Draw();
        }

        private void InitGui()
        {
            int blocksX = 10;
            int blocksY = 10;
            int blockSize = 20;
            bool success = int.TryParse(Tools.ReadSetting(Tools.BlockCountX), out blocksX);
            success = int.TryParse(Tools.ReadSetting(Tools.BlockCountY), out blocksY);
            success = int.TryParse(Tools.ReadSetting(Tools.BlockSize), out blockSize);            

            if (!success)
            {
            }

            _board = new Board(blocksX, blocksY, blockSize);
            var size = _board.GetWindowSize();
            this.Width = size.Width;
            this.Height = size.Height;
            this.Content = _board.Canvas;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.MouseDevice.GetPosition(_board.Canvas);
            _board.GetCell((int)position.X, (int)position.Y, 1);
            _board.Draw();
        }
    }
}
