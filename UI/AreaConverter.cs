using System;
using System.Globalization;
using System.Windows.Data;

namespace UI
{
    public class AreaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var area = values[0] as string;
            var parentArea = values[1] as string;

            return string.IsNullOrEmpty(area) ? parentArea : area;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
