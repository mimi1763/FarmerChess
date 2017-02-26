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
            int blocksX = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountX, true);
            int blocksY = (int)Tools.ReadSetting(Tools.SettingsKey_BlockCountY, true);
            int blockSize = (int)Tools.ReadSetting(Tools.SettingsKey_BlockSize, true);            

            _board = new Board(blocksX, blocksY, blockSize);
            var size = _board.GetWindowSize();
            this.Width = size.Width;
            this.Height = size.Height;
            this.Content = _board.Canvas;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCellAtMousePos(e);
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCellAtMousePos(e, true);
        }

        private void ChangeCellAtMousePos(MouseButtonEventArgs e, bool isRightButton = false)
        {
            var position = e.MouseDevice.GetPosition(_board.Canvas);
            int player = isRightButton ? (int)Board.Player.X : (int)Board.Player.O;
            var cell = _board.GetCell((int)position.X, (int)position.Y, player);
            _board.DrawCell(cell);
        }
    }
}
