using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Farmerchess
{
    public sealed class Tools
    {
        private static volatile Tools instance;
        private static object syncRoot = new Object();

        public static readonly string SettingsKey_BlockCountX = "BlockCountX";
        public static readonly string SettingsKey_BlockCountY = "BlockCountY";
        public static readonly string SettingsKey_BlockSize = "BlockSize";
        public static readonly string SettingsKey_LineThickness = "LineThickness";
        public static readonly string SettingsKey_BgColour = "BgColour";
        public static readonly string SettingsKey_OColour = "OColour";
        public static readonly string SettingsKey_XColour = "XColour";
        public static readonly string SettingsKey_GridColour = "GridColour";

        private Tools()
        {
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
            return value != null ? new SolidColorBrush((Color)value) : Brushes.Red;
        }
    }
}
