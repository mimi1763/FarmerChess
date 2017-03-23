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

        public readonly string SettingsKey_BlockCountX = "BlockCountX";
        public readonly string SettingsKey_BlockCountY = "BlockCountY";
        public readonly string SettingsKey_BlockSize = "BlockSize";
        public readonly string SettingsKey_LineThickness = "LineThickness";
        public readonly string SettingsKey_BgColour = "BgColour";
        public readonly string SettingsKey_OColour = "OColour";
        public readonly string SettingsKey_XColour = "XColour";
        public readonly string SettingsKey_GridColour = "GridColour";
        public readonly string SettingsKey_XImagePath = "XImagePath";
        public readonly string SettingsKey_OImagePath = "OImagePath";
        public readonly string SettingsKey_UseTestGrid = "UseTestGrid";

        public ImageSource ImageX;
        public ImageSource ImageO;

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
            //LoadImages();
        }

        /// <summary>
        /// Read more here: http://stackoverflow.com/questions/14336597/adding-image-objects-to-wpf-with-code
        /// </summary>
        private void LoadImages()
        {
            var pathX = (string)ReadSetting(SettingsKey_XImagePath);
            var pathO = (string)ReadSetting(SettingsKey_OImagePath);
            ImageX = new BitmapImage(new Uri(pathX));
            ImageO = new BitmapImage(new Uri(pathO));
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

        public static object ReadSetting(string key, bool isNumber = false)
        {
            try
            {
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

        public static SolidColorBrush GetBrush(string colourString)
        {
            var setting = (string)ReadSetting(colourString);
            ColorConverter converter = new ColorConverter();
            object value = null;
            try
            {
                value = converter.ConvertFromInvariantString(setting);
            }
            catch (Exception) {}
            return value != null ? new SolidColorBrush((Color)value) : Brushes.Black;
        }
    }
}
