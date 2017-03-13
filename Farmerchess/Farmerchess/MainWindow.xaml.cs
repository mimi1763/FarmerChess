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
            var useTestGrid = (int)Tools.ReadSetting(Tools.SettingsKey_UseTestGrid, true);
            InitGame();
            _game.Draw();
        }

        private void InitGame()
        {           
            _game = new Game();
            var size = _game.GameWindowSize;
            this.Content = _game.Canvas;
            this.Width = size.Width;
            this.Height = size.Height;

            //_game.Start();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _game.ChangeCellAtMousePos(e.MouseDevice);
        }
    }
}
