using System.Data;

/******************************************************************************************************************
 * 
 * 
 * 说　明： 类型转换(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/08/29
 * 修　改：
 * 参　考：http://www.cnblogs.com/Relict/archive/2011/12/19/2293460.html
 * 备　注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public class CodeCommon
    {
        /// <summary>
        /// SqlDbType转换为C#数据类型
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static Type SqlType2CsharpType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Object);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.Real:
                    return typeof(Single);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.UniqueIdentifier:
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    return null;
            }
        }

        /// <summary>
        /// sql server数据类型（如：varchar）转换为SqlDbType类型
        /// </summary>
        /// <param name="sqlTypeString"></param>
        /// <returns></returns>
        public static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        /// <summary>
        /// sql server中的数据类型，转换为C#中的类型类型
        /// </summary>
        /// <param name="sqlTypeString"></param>
        /// <returns></returns>
        public static Type SqlTypeString2CsharpType(string sqlTypeString)
        {
            SqlDbType dbTpe = SqlTypeString2SqlType(sqlTypeString);

            return SqlType2CsharpType(dbTpe);
        }

        /// <summary>
        /// 将sql server中的数据类型，转化为C#中的类型的字符串
        /// </summary>
        /// <param name="sqlTypeString"></param>
        /// <returns></returns>
        public static string SqlTypeString2CsharpTypeString(string sqlTypeString)
        {
            Type type = SqlTypeString2CsharpType(sqlTypeString);

            return type.Name;
        }

        /// <summary> 
        /// 将.NET数据类型（如：System.Int32）转换为C#类型（如：int） 
        /// </summary> 
        /// <param name="dotNetString">.NET数据类型</param> 
        /// <returns></returns> 
        public static string DotNetTypeToCSType(string dotNetString)
        {
            string[] dotNetTypes = { "system.boolean", "system.char","system.byte" ,"system.sbyte",
                "system.uint16","system.uint32","system.uint64","system.int16","system.int32","system.int64","system.single","system.double", "system.string"};

            string[] csTypes = {"bool", "char","byte" ,"sbyte","ushort","uint","ulong","short",
                "int","long","float","double", "string"};

            int i = Array.IndexOf(dotNetTypes, dotNetString.ToLower());
            if (i > -1)
            {
                return csTypes[i];
            }
            else
            {
                return dotNetString.ToLower();
            }

        }
        /// <summary> 
        /// 将SQLServer数据类型（如：varchar）转换为.Net类型（如：String） 
        /// </summary> 
        /// <param name="sqlTypeString">SQLServer数据类型</param> 
        /// <returns></returns> 
        public static string DbTypeToCS(string sqlTypeString)
        {
            string[] sqlTypeNames = { "int", "integer", "varchar","bit" ,"datetime","decimal","float","image","money",
                "ntext","nvarchar","smalldatetime","smallint","text","bigint","binary","char","nchar","numeric",
                "real","smallmoney", "sql_variant","timestamp","tinyint","uniqueidentifier","varbinary","mediumint","mediumtext"};

            string[] dotNetTypes = {"int", "int", "string","bool" ,"DateTime","Decimal","Double","Byte[]","Decimal",
                "string","string","DateTime","Int16","string","Int64","Byte[]","string","string","Decimal",
                "Single","Single", "Object","Byte[]","Byte","Guid","Byte[]","int","string"};
            int i = Array.IndexOf(sqlTypeNames, sqlTypeString.ToLower());
            if (i > -1)
            {
                return dotNetTypes[i];
            }
            else
            {
                return sqlTypeString.ToLower();
            }

        }
        /// <summary> 
        /// 将SQLServer数据类型（如：varchar）转换为DbType（如：String） 
        /// </summary> 
        /// <param name="sqlTypeString">SQLServer数据类型</param> 
        /// <returns></returns> 
        public static string SqlTypeToDbType(string sqlTypeString)
        {
            string[] sqlTypeNames = { "int", "integer", "varchar","bit" ,"datetime","decimal","float","image","money",
                "ntext","nvarchar","smalldatetime","smallint","text","bigint","binary","char","nchar","numeric",
                "real","smallmoney", "sql_variant","timestamp","tinyint","uniqueidentifier","varbinary"};

            string[] dotNetTypes = {"Int32", "Int32", "String","Boolean" ,"DateTime","Decimal","Double","Byte[]","Single",
                "String","String","DateTime","Int16","String","Int64","Byte[]","String","String","Decimal",
                "Single","Single", "Object","Byte[]","Byte","Guid","Byte[]"};
            int i = Array.IndexOf(sqlTypeNames, sqlTypeString.ToLower());
            if (i > -1)
            {
                return dotNetTypes[i];
            }
            else
            {
                return sqlTypeString.ToLower();
            }

        }
    }
}