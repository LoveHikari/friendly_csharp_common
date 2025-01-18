using System;
using System.Globalization;
using System.Windows.Data;

namespace Hikari.Mvvm.Converter;
/// <summary>
/// 取反bool
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool booleanValue)
        {
            return !booleanValue; // 取反
        }
        return false; // 默认返回 false
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}