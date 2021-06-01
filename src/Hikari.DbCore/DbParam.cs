using System;
using System.Data;

namespace Hikari.DbCore
{
    /// <summary>
    /// DbCommand 的参数
    /// </summary>
    [Serializable]
    public sealed class DbParam
    {
        /// <summary>
        /// DbCommand 的参数
        /// </summary>
        public DbParam()
        {
        }

        /// <summary>
        /// DbCommand 的参数
        /// </summary>
        /// <param name="fields">参数名称</param>
        /// <param name="dbValue">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数最大长度</param>
        /// <param name="direction">查询相关参数类型</param>
        public DbParam(string fields, Object dbValue, DbType dbType, int size = 255, ParameterDirection direction = ParameterDirection.Input)
        {
            this.FieldName = fields;
            this.DbValue = dbValue;
            this.DbType = dbType;
            this.Size = size;
            this.Direction = direction;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        internal string FieldName { get; private set; }

        /// <summary>
        /// 参数值
        /// </summary>
        internal Object DbValue { get; private set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        internal DbType DbType { get; private set; }
        /// <summary>
        /// 参数最大长度
        /// </summary>
        internal int Size { get; private set; }
        /// <summary>
        /// 查询相关参数类型
        /// </summary>
        internal ParameterDirection Direction { get; private set; }
    }
}