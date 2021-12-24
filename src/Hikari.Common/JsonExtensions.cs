﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Text.Json.JsonElement;

namespace Hikari.Common
{
    /// <summary>
    /// <see cref="System.Text.Json"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class JsonExtensions
    {
        /// <summary>
        /// 将表示单个 JSON 值的文本解析为由泛型类型参数指定的类型的实例。
        /// </summary>
        /// <typeparam name="TValue">JSON 值的目标类型</typeparam>
        /// <param name="jsonArray">要解析的 JSON 文本</param>
        /// <param name="options">在解析过程中控制行为的选项</param>
        /// <returns>JSON 值的 TValue 表示</returns>
        public static List<TValue> Deserialize<TValue>(this in ArrayEnumerator jsonArray, JsonSerializerOptions? options = null)
        {
            List<TValue> resultData = new List<TValue>();
            foreach (var item in jsonArray)
            {
                var v = JsonSerializer.Deserialize<TValue>(item.GetString()??"", options);
                resultData.Add(v);
            }
            return resultData;
        }
    }
}
