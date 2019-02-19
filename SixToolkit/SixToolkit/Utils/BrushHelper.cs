using System;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Utils
{
    public static class BrushHelper
    {
        public static SolidColorBrush ToBrush(this string hex)
        {
            try
            {
                if (hex == null || !hex.StartsWith("#") || (hex.Length != 7 && hex.Length != 9))
                {
                    return null;
                }

                byte alpha = 255;
                var offset = 0;
                if (hex.Length == 9)
                {
                    alpha = (byte)(Convert.ToUInt32(hex.Substring(1, 2), 16));
                    offset = 2;
                }

                var r = (byte)(Convert.ToUInt32(hex.Substring(offset + 1, 2), 16));
                var g = (byte)(Convert.ToUInt32(hex.Substring(offset + 3, 2), 16));
                var b = (byte)(Convert.ToUInt32(hex.Substring(offset + 5, 2), 16));
                return new SolidColorBrush(Windows.UI.Color.FromArgb(alpha, r, g, b));
            }
            catch
            {
                return null;
            }
        }
    }
}
