using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Farmerchess.Gui;
using System.Windows.Media.Imaging;

namespace Farmerchess
{
    public sealed class Tools
    {
        private static volatile Tools instance;
        private static object syncRoot = new Object();

        public readonly string SettingsKey_BlockCountX = "nbrBlockCountX";
        public readonly string SettingsKey_BlockCountY = "nbrBlockCountY";
        public readonly string SettingsKey_BlockSize = "nbrBlockSize";
        public readonly string SettingsKey_LineThickness = "nbrLineThickness";
        public readonly string SettingsKey_BgColour = "BgColour";
        public readonly string SettingsKey_GridColour = "GridColour";
        public readonly string SettingsKey_XImagePath = "XImagePath";
        public readonly string SettingsKey_OImagePath = "OImagePath";
        public readonly string SettingsKey_UseTestGrid = "nbrUseTestGrid";
        public readonly string SettingsKey_DebugMode = "nbrDebugMode";

        public static ImageBrush ImageX { get; private set; }
        public static ImageBrush ImageO { get; private set; }

        //Test grid 10x10
        public int[,] TestGridO = new int[,]  { { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                         { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }};

        public int[,] TestGridX = new int[,]  { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        private Tools()
        {
            LoadImages();
        }

        /// <summary>
        /// Read more here: http://stackoverflow.com/questions/14336597/adding-image-objects-to-wpf-with-code
        /// </summary>
        private void LoadImages()
        {
            try
            {
                var blockSize = (int)ReadSetting(SettingsKey_BlockSize);
                var pathX = (string)ReadSetting(SettingsKey_XImagePath);
                var pathO = (string)ReadSetting(SettingsKey_OImagePath);
                var imageSrcX = BitmapFrame.Create(new Uri(pathX, UriKind.RelativeOrAbsolute));
                var imageSrcO = BitmapFrame.Create(new Uri(pathO, UriKind.RelativeOrAbsolute));
                //double scale = imageSrcO.Width / blockSize;
                ImageX = new ImageBrush(imageSrcX);
                ImageO = new ImageBrush(imageSrcO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Tools Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Tools();
                    }
                }

                return instance;
            }
        }

        public enum Player
        {
            Empty = 0,
            X = 1,
            O = 2
        }

        public object ReadSetting(string key)
        {
            try
            {
                var isNumber = key.Contains("nbr");
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings[key];
                if (isNumber)
                {
                    int number;
                    bool success = int.TryParse(value, out number);
                    return success ? number : -1;
                }
                return value;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return null;
        }

        public SolidColorBrush GetBrush(string colourString)
        {
            var setting = (string)ReadSetting(colourString);
            ColorConverter converter = new ColorConverter();
            object value = null;
            try
            {
                value = converter.ConvertFromInvariantString(setting);
            }
            catch (Exception) { }
            return value != null ? new SolidColorBrush((Color)value) : Brushes.Black;
        }

        public ImageBrush GetImageBrush(int player)
        {
            return player == (int)Player.O ? ImageO : ImageX;
        }
    }
}
