using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalyzerDatabase.Converters
{
    public class DuplicateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().StartsWith(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}