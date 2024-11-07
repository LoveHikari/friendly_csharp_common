using System;
using System.Globalization;
using System.Windows.Data;

namespace Hikari.Mvvm.Converter;

[ValueConversion(typeof(int), typeof(bool))]
public class IntToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue != 0; // 非零为 true，零为 false
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? 1 : 0; // true 转为 1，false 转为 0
        }
        return 0;
    }
}