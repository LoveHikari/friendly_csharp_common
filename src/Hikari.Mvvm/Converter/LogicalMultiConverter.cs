using System;
using System.Globalization;
using System.Windows.Data;
/***
<Window.Resources>
    <local:LogicalMultiConverter x:Key="LogicalConverter" />
</Window.Resources>

<!-- AND逻辑示例 -->
<Button Content="提交">
    <Button.IsEnabled>
        <MultiBinding Converter="{StaticResource LogicalConverter}" ConverterParameter="AND">
            <Binding Path="IsCheckbox1Checked" />
            <Binding Path="IsCheckbox2Checked" />
        </MultiBinding>
    </Button.IsEnabled>
</Button>

<!-- OR逻辑示例 -->
<TextBlock Foreground="Red">
    <TextBlock.Visibility>
        <MultiBinding Converter="{StaticResource LogicalConverter}" ConverterParameter="OR">
            <Binding Path="HasError" />
            <Binding Path="IsWarning" />
        </MultiBinding>
    </TextBlock.Visibility>
</TextBlock>
***/
namespace Hikari.Mvvm.Converter;
/// <summary>
/// 支持逻辑类型的多值转换器​
/// 解析ConverterParameter（默认为AND逻辑）
/// </summary>
public class LogicalMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length == 0)
            return false;

        // 解析ConverterParameter（默认为AND逻辑）
        string logicType = parameter?.ToString()?.ToUpper() ?? "AND";
        bool isAndLogic = logicType == "AND";

        foreach (var value in values)
        {
            bool isTrue = value is bool b && b;

            // OR逻辑：任意一个为true立即返回true
            if (!isAndLogic && isTrue)
                return true;

            // AND逻辑：任意一个为false立即返回false
            if (isAndLogic && !isTrue)
                return false;
        }

        return isAndLogic; // AND逻辑全部为true返回true，OR逻辑全部为false返回false
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // 单向绑定无需实现
    }
}