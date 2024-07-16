using System;
using System.Globalization;
using System.Windows.Data;

namespace UI
{
    public class ContainerIdToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int containerId = (int)value;
            int currentContainerId = (int)parameter;
            return containerId != currentContainerId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
