using System.Data;
using System.Reflection;
using System.Text;

/******************************************************************************************************************
 * 
 * 
 * 标  题： DataTable 扩展类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/05/12
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common;
/// <summary>
/// <see cref="DataTable"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class DataTableExtensions
{
    /// <summary>
    /// <see cref="DataTable"/> 转 <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="dt"><see cref="DataTable"/> 数据</param>
    /// <returns>模型列表</returns>
    public static List<T> ToList<T>(this DataTable dt) where T : class, new()
    {
        // 定义集合 
        List<T> ts = new List<T>();

        //遍历DataTable中所有的数据行 
        foreach (DataRow dr in dt.Rows)
        {
            var t = dr.ToEntity<T>();
            //对象添加到泛型集合中 
            ts.Add(t);
        }
        return ts;

    }
    /// <summary>   
    /// 将实体类列表转换成 <see cref="DataTable"/>
    /// </summary>   
    /// <typeparam name="T">类型</typeparam>
    /// <param name="objList">实体</param>
    /// <returns><see cref="DataTable"/></returns>
    public static DataTable? ToDataTable<T>(this IList<T>? objList)
    {
        if (objList is not { Count: > 0 })
        {
            return null;
        }
        DataTable dt = new DataTable(typeof(T).Name);

        System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (T t in objList)
        {
            if (t == null)
            {
                continue;
            }

            var row = dt.NewRow();

            for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
            {
                System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                string name = pi.Name;

                if (dt.Columns[name] == null)
                {
                    var column = new DataColumn(name, pi.PropertyType);
                    dt.Columns.Add(column);
                }

                row[name] = pi.GetValue(t, null);
            }

            dt.Rows.Add(row);
        }
        return dt;
    }
    /// <summary>
    /// 将 <see cref="DataRow"/> 转成实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="dr"><see cref="DataRow"/> 数据</param>
    /// <returns>实体</returns>
    public static T ToEntity<T>(this DataRow dr) where T : class, new()
    {
        T t = new T();
        // 获得此模型的公共属性 
        PropertyInfo[] propertys = t.GetType().GetProperties();

        //遍历该对象的所有属性 
        foreach (PropertyInfo pi in propertys)
        {
            string tempName = pi.Name;//将属性名称赋值给临时变量
                                      //检查DataTable是否包含此列（列名==对象的属性名）  
            if (dr.Table.Columns.Contains(tempName))
            {
                //取值
                object value = dr[tempName];
                //如果非空，则赋给对象的属性 
                pi.SetValue(t, value.ChangeTypeTo(pi.PropertyType), null);


            }

        }

        return t;
    }
    /// <summary>
    /// 统计指定列的数据的和
    /// </summary>
    /// <param name="dt">数据表</param>
    /// <param name="columnName">列名</param>
    /// <returns></returns>
    public static double ColumnSum(this DataTable dt, string columnName)
    {
        double d = 0;
        foreach (DataRow row in dt.Rows)
        {
            d += row[columnName].ToDouble() ?? 0D;
        }
        return d;
    }
    /// <summary>
    /// 将<see cref="DataColumn"/> 转为集合
    /// </summary>
    /// <typeparam name="T">指定集合类型</typeparam>
    /// <param name="dc"><see cref="DataColumn"/> 数据</param>
    /// <returns></returns>
    public static List<T> ToList<T>(this DataColumn dc)
    {
        DataTable dt = dc.Table!;
        string columnName = dc.ColumnName;
        List<T> list = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add((T)Convert.ChangeType(row[columnName], typeof(T)));
        }
        return list;
    }
    /// <summary>  
    /// 分解数据表  
    /// </summary>  
    /// <param name="originalTab">需要分解的表</param>  
    /// <param name="rowsNum">每个表包含的数据量</param>  
    /// <returns></returns>
    public static DataSet SplitDataTable(this DataTable originalTab, int rowsNum)
    {
        //获取所需创建的表数量  
        int tableNum = Math.Ceiling(originalTab.Rows.Count / (double)rowsNum).ToInt32() ?? 0;

        //获取数据余数  
        int remainder = originalTab.Rows.Count % rowsNum;

        DataSet ds = new DataSet();

        //如果只需要创建1个表，直接将原始表存入DataSet
        if (tableNum == 0)
        {
            ds.Tables.Add(originalTab);
        }
        else
        {
            DataTable[] tableSlice = new DataTable[tableNum];

            //将原始列保存到新表中
            for (int c = 0; c < tableNum; c++)
            {
                tableSlice[c] = new DataTable();
                foreach (DataColumn dc in originalTab.Columns)
                {
                    tableSlice[c].Columns.Add(dc.ColumnName, dc.DataType);
                }
            }
            //导入行
            for (int i = 0; i < tableNum; i++)
            {
                // 如果当前表不是最后一个表
                if (i != tableNum - 1)
                {
                    for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                    {
                        tableSlice[i].ImportRow(originalTab.Rows[j]);
                    }
                }
                else
                {
                    for (int k = i * rowsNum; k < originalTab.Rows.Count; k++)
                    {
                        tableSlice[i].ImportRow(originalTab.Rows[k]);
                    }
                }
            }

            //将所有表添加到数据集中
            foreach (DataTable dt in tableSlice)
            {
                ds.Tables.Add(dt);
            }
        }
        return ds;
    }

    /// <summary>
    /// DataTable转json
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToJson(this DataTable dt)
    {
        StringBuilder json = new StringBuilder();
        json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    json.Append("\"" + dt.Columns[j].ColumnName + "\":\"" + dt.Rows[i][j].ToString()?.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("“", "").Replace("”", "").Replace(" ", "").Replace("\\", "").Replace("	", "") + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        json.Append(",");
                    }
                }
                json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    json.Append(",");
                }
            }
        }
        json.Append("]");
        return json.ToString();
    }
}
