using System.Reflection;
using Spire.Doc;
using Spire.Xls;

namespace Hikari.Common.Office;
/// <summary>
/// SpireOffice激活
/// </summary>
public static class SpireOfficeHelper
{
    /// <summary>
    /// 注入激活信息
    /// </summary>
    /// <param name="workbook"></param>
    public static void Crack(this Workbook workbook)
    {
        CrackLicense(workbook);
    }
    /// <summary>
    /// 注入激活信息
    /// </summary>
    /// <param name="document"></param>
    public static void Crack(this Document document)
    {
        CrackLicense(document);
    }
    /// <summary>
    /// 注入激活信息，并返回该类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    private static T CrackLicense<T>(T t) where T : class
    {
        var internalLicense = t.GetType().GetProperty("InternalLicense", BindingFlags.NonPublic | BindingFlags.Instance);
        var typeLic = internalLicense.PropertyType.Assembly.CreateInstance(internalLicense.PropertyType.GetTypeInfo().FullName);
        foreach (var item in typeLic.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (item.FieldType.IsArray)
            {
                item.SetValue(typeLic, new string[] { "Spire.Spreadsheet", "Spire.DocViewer.Wpf" });
            }
            else if (item.FieldType.IsEnum)
            {
                item.SetValue(typeLic, 3);
            }
        }
        internalLicense.SetValue(t, typeLic);
        return t;
    }


}