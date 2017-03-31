using Farmerchess.Gui;
using System.Windows;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Numerics;

namespace Farmerchess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game _game;

        public MainWindow()
        {
            InitializeComponent();
            var useTestGrid = (int)Tools.Instance.ReadSetting(Tools.Instance.SettingsKey_UseTestGrid);
            InitGame();
            _game.Draw();
        }

        private void InitGame()
        {           
            _game = new Game();
            this.Content = _game.CanvasList[0];
            this.Width = _game.GameWindowSize.Width;
            this.Height = _game.GameWindowSize.Height;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _game.ChangeCellAtMousePos(e.MouseDevice);
        }
    }
}
