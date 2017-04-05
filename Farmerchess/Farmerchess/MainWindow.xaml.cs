using Farmerchess.Gui;
using System.Windows;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Numerics;
using System.ComponentModel;

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
            this.Content = _game.BoardList[0].Canvas;
            this.Width = _game.GameWindowSize.Width;
            this.Height = _game.GameWindowSize.Height;
            this.Closing += OnWindowClose;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _game.ChangeCellAtMousePos(e.MouseDevice);
        }

        public void OnWindowClose(object sender, CancelEventArgs e)
        {
            _game.CloseDebugWindow();
        }
    }
}
