using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Hikari.Mvvm.Converter;

/// <summary>
/// 用于从Xml中AddCoilCommand事件命令传递多参数转换
/// </summary>
public class MultiValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.ToArray();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}