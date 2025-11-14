namespace Hikari.Common;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class HexExtensions
{
    /// <param name="bytes"></param>
    extension(byte[] bytes)
    {
        /// <summary>
        /// 转换：支持 0x、空格、分隔符
        /// </summary>
        /// <param name="upperCase">是否大写</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string ToHexString(bool upperCase = true)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return string.Empty;

            var format = upperCase ? "X2" : "x2";
            return string.Create(bytes.Length * 2, bytes, (span, data) =>
            {
                for (int i = 0; i < data.Length; i++)
                    data[i].TryFormat(span.Slice(i * 2, 2), out _, format);
            });
        }

        /// <summary>
        /// 转换：支持 0x、空格、分隔符
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <param name="upperCase">是否大写</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string ToHexString(string separator, bool upperCase = true)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return string.Empty;
            if (string.IsNullOrEmpty(separator)) return bytes.ToHexString(upperCase);

            var format = upperCase ? "X2" : "x2";
            return string.Join(separator, Array.ConvertAll(bytes, b => b.ToString(format)));
        }
    }

    /// <summary>
    /// 解析：支持 0x、空格、分隔符
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static byte[] FromHexString(this string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return Array.Empty<byte>();
        hex = hex.Replace(" ", "").Replace("-", "").Replace(":", "");
        if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            hex = hex[2..];

        if (hex.Length % 2 != 0)
            throw new FormatException("长度必须为偶数");

        var result = new byte[hex.Length / 2];
        for (int i = 0; i < result.Length; i++)
            result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return result;
    }
}