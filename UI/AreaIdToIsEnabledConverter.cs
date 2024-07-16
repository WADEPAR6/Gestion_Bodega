using System;
using System.Globalization;
using System.Windows.Data;

namespace UI
{
    public class AreaIdToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int areaId = (int)value;
            int currentAreaId = (int)parameter;
            return areaId != currentAreaId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
