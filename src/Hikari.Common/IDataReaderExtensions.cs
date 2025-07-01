using System.Data;
using System.Reflection;

/******************************************************************************************************************
 * 
 * 
 * 标  题： IDataReader 扩展类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/05/31
 * 修  改：
 * 参  考： http://www.thinksaas.cn/topics/0/568/568989.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common;
/// <summary>
/// <see cref="IDataReader"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class IDataReaderExtensions
{
    /// <summary>
    ///  将IDataReader转换为DataTable
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static DataTable ToDataTable(this IDataReader reader)
    {
        DataTable objDataTable = new DataTable("Table1");
        int intFieldCount = reader.FieldCount;
        for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
        {
            objDataTable.Columns.Add(reader.GetName(intCounter).ToUpper(), reader.GetFieldType(intCounter));
        }
        objDataTable.BeginLoadData();
        object[] objValues = new object[intFieldCount];
        while (reader.Read())
        {
            reader.GetValues(objValues);
            objDataTable.LoadDataRow(objValues, true);
        }
        reader.Close();
        objDataTable.EndLoadData();
        return objDataTable;
    }
    /// <summary>
    /// 将IDataReader转换为实体类型
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static T? ToEntity<T>(this IDataReader reader)
    {
        using (reader)
        {
            if (reader.Read())
            {
                List<string> list = new List<string>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    list.Add(reader.GetName(i).ToLower());
                }
                T model = Activator.CreateInstance<T>();
                foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (list.Contains(pi.Name.ToLower()))
                    {
                        if (!IsNullOrDbNull(reader[pi.Name]))
                        {
                            pi.SetValue(model, HackType(reader[pi.Name], pi.PropertyType), null);
                        }
                    }
                }
                return model;
            }
        }
        return default(T);
    }
    /// <summary>
    /// 将IDataReader转换为List
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this IDataReader reader)
    {
        using (reader)
        {
            List<string> field = new List<string>(reader.FieldCount);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                field.Add(reader.GetName(i).ToLower());
            }
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (field.Contains(property.Name.ToLower()))
                    {
                        if (!IsNullOrDbNull(reader[property.Name]))
                        {
                            property.SetValue(model, HackType(reader[property.Name], property.PropertyType), null);
                        }
                    }
                }
                list.Add(model);
            }
            return list;
        }
    }

    //这个类对可空类型进行判断转换，要不然会报错
    private static object? HackType(object? value, Type conversionType)
    {
        if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            if (value == null)
                return null;

            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
            conversionType = nullableConverter.UnderlyingType;
        }
        return Convert.ChangeType(value, conversionType);
    }
    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static bool IsNullOrDbNull(object? obj)
    {
        return ((obj is DBNull) || string.IsNullOrEmpty(obj?.ToString()));
    }
}

