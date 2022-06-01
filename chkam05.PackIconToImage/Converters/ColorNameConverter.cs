using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace chkam05.PackIconToImage.Converters
{
    public class ColorNameConverter : IValueConverter
    {

        //  CONST

        public static readonly Dictionary<string, Color> Colors = typeof(Colors)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(k => k.Name, v => (Color)v.GetValue(null));


        //  METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Converts a value. </summary>
        /// <param name="value"> Value produced by the binding source. </param>
        /// <param name="targetType"> Type of the binding target property. </param>
        /// <param name="parameter"> Converter parameter to use. </param>
        /// <param name="culture"> Culture to use in the converter. </param>
        /// <returns> Converted value. If the method returns null, the valid null value is used. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;

            if (Colors.Any(c => c.Value == color))
                return Colors.First(c => c.Value == color).Key;

            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Converts a value. </summary>
        /// <param name="value"> Value that is produced by the binding target. </param>
        /// <param name="targetType"> Type to convert to. </param>
        /// <param name="parameter"> Converter parameter to use. </param>
        /// <param name="culture"> Culture to use in the converter. </param>
        /// <returns> Converted value. If the method returns null, the valid null value is used. </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as string;

            try
            {
                if (!string.IsNullOrEmpty(color))
                {
                    if (Colors.ContainsKey(color))
                        return Colors[color];

                    return ColorConverter.ConvertFromString(color);
                }
            }
            catch
            {
                //
            }

            return System.Windows.Media.Colors.Transparent;
        }

    }
}
