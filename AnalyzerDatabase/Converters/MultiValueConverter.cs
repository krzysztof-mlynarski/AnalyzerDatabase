using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalyzerDatabase.Converters
{
    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string one = values[0] as string;
            string two = values[1] as string;
            if (!string.IsNullOrEmpty(one) && !string.IsNullOrEmpty(two))
            {
                return one + two;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}