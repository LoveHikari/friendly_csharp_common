using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hikari.Common.Text.Json
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
        public static List<TValue> Deserialize<TValue>(this in JsonElement.ArrayEnumerator jsonArray, JsonSerializerOptions? options = null) where TValue : class
        {
            List<TValue> resultData = new List<TValue>();
            foreach (var item in jsonArray)
            {
                if (typeof(TValue) == typeof(string))
                {
                    resultData.Add(item.ToString() as TValue);
                }
                else
                {
                    var v = JsonSerializer.Deserialize<TValue>(item.ToString(), options);
                    resultData.Add(v);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 设置默认如何忽略 https://stackoverflow.com/questions/58331479/how-to-globally-set-default-options-for-system-text-json-jsonserializer
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition"></param>
        public static void SetIgnoreCondition(this JsonSerializerOptions @this, JsonIgnoreCondition condition)
        {
            typeof(JsonSerializerOptions).GetRuntimeFields()
                .Single(f => f.Name == "_defaultIgnoreCondition")
                .SetValue(JsonSerializerOptions.Default, condition);

        }
        /// <summary>
        /// 设置默认配置
        /// </summary>
        /// <param name="this"></param>
        /// <param name="options"></param>
        public static void SetDefaultOptions(this JsonSerializerOptions @this, JsonSerializerOptions options)
        {
            typeof(JsonSerializerOptions).GetRuntimeFields()
                .Single(f => f.Name == "s_defaultOptions")
                .SetValue(JsonSerializerOptions.Default, options);

        }

    }
}
