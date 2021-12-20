using System;
using System.Globalization;
using System.Windows.Data;

namespace BrokerAppTest.Mvvm.Converters
{
    public class OnSaleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Продажа" : "Покупка";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
