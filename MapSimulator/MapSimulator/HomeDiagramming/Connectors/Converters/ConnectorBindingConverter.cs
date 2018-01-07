using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HomeDiagramming.Connectors.Converters
{
    public class ConnectorBindingConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double left = System.Convert.ToDouble(values[0]);
            double top = System.Convert.ToDouble(values[1]);
            double actualWidth = System.Convert.ToDouble(values[2]);
            double actualHeight = System.Convert.ToDouble(values[3]);
            return new Point(left + actualWidth / 2, top + actualHeight / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
