using System.Text.Json;

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
    }
}
