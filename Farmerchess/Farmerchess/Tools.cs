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

        public static string BlockCountX = "BlockCountX";
        public static string BlockCountY = "BlockCountY";
        public static string BlockSize = "BlockSize";
        public static string LineThickness = "LineThickness";
        public static string BgColour = "BgColour";
        public static string OColour = "OColour";
        public static string XColour = "XColour";
        public static string GridColour = "GridColour";

        private Tools()
        {
        }

        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? null;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return null;
        }

        public static SolidColorBrush GetBrush(string colourString)
        {
            var setting = ReadSetting(colourString);
            ColorConverter converter = new ColorConverter();
            object value = null;
            try
            {
                value = converter.ConvertFromInvariantString(setting);
            }
            catch (Exception) {}
            return value != null ? new SolidColorBrush((Color)value) : Brushes.Red;
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
    }
}
