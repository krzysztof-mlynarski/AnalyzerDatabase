using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AnalyzerDatabase.Converters
{
    public class ViewModelBaseToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility) value == Visibility.Collapsed;
        }
    }
}