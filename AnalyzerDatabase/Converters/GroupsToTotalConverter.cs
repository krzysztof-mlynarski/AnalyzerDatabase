using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace AnalyzerDatabase.Converters
{
    public class GroupsToTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<Object>)
            {
                var items = (ReadOnlyObservableCollection<Object>)value;
                Decimal total = 0;
                foreach (GroupItem gi in items)
                {
                    total += gi.TabIndex;
                }
                return total.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}