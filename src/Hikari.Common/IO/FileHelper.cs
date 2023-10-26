/******************************************************************************************************************
 * 
 * 
 * 标  题：文件帮助类(版本：Version1.0.2)
 * 作  者：YuXiaoWei
 * 日  期：2016/10/20
 * 修  改：2016/10/31, 2021/12/15
 * 参  考：
 * 说  明：暂无...
 * 备  注：暂无...
 * 
 * 
 * ***************************************************************************************************************/


namespace Hikari.Common.IO;

/// <summary>
/// 文件帮助类
/// </summary>
public class FileHelper
{
    #region 写文件
    /// <summary>
    /// 写文件，如果文件不存在则创建，存在则覆盖
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="strings">文件内容</param>
    /// <param name="charset">编码，默认utf-8</param>
    public static async Task WriteAsync(string path, string strings, string charset = "utf-8")
    {
        Create(path);

        await using System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding(charset));
        await f2.WriteAsync(strings);
    }
    /// <summary>
    /// 写文件，如果文件不存在则创建，存在则覆盖
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="strings">文件内容</param>
    /// <param name="charset">编码，默认utf-8</param>
    public static void Write(string path, string strings, string charset = "utf-8")
    {
        Create(path);

        using System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding(charset));
        f2.Write(strings);
    }
    /// <summary>
    /// 写文件，如果文件不存在则创建，存在则追加
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="strings">内容</param>
    /// <param name="charset">编码，默认utf-8</param>
    public static async Task AppendAsync(string path, string strings, string charset = "utf-8")
    {
        Create(path);

        await using System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, true, System.Text.Encoding.GetEncoding(charset));
        await f2.WriteAsync(strings);
    }
    /// <summary>
    /// 写文件，如果文件不存在则创建，存在则追加
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="strings">内容</param>
    /// <param name="charset">编码，默认utf-8</param>
    public static void Append(string path, string strings, string charset = "utf-8")
    {
        Create(path);

        using System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, true, System.Text.Encoding.GetEncoding(charset));
        f2.Write(strings);
    }
    /// <summary>
    ///  将 Stream 写入文件 
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="stream"></param>
    public static async Task WriteAsync(string filePath, Stream stream)
    {
        Create(filePath);

        // 把 Stream 转换成 byte[] 
        byte[] bytes = new byte[stream.Length];
        await stream.ReadAsync(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始 
        stream.Seek(0, SeekOrigin.Begin);
        // 把 byte[] 写入文件 
        await using FileStream fs = new FileStream(filePath, FileMode.Create);
        await using BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(bytes);
    }
    /// <summary>
    ///  将 Stream 写入文件 
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="stream"></param>
    public static void Write(string filePath, Stream stream)
    {
        Create(filePath);

        // 把 Stream 转换成 byte[] 
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始 
        stream.Seek(0, SeekOrigin.Begin);
        // 把 byte[] 写入文件 
        using FileStream fs = new FileStream(filePath, FileMode.Create);
        using BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(bytes);
    }
    #endregion

    #region 读文件

    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="charset">编码，默认utf-8</param>
    /// <returns></returns>
    public static async Task<string> ReadAsync(string path, string charset = "utf-8")
    {
        using System.IO.StreamReader f2 = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(charset));
        var s = await f2.ReadToEndAsync();
        return s;
    }
    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="charset">编码，默认utf-8</param>
    /// <returns></returns>
    public static string Read(string path, string charset = "utf-8")
    {
        using System.IO.StreamReader f2 = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(charset));
        var s = f2.ReadToEnd();
        return s;
    }
    /// <summary>
    /// 从文件读取 Stream 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<Stream> ReadFileAsync(string filePath)
    {
        // 打开文件 
        await using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        // 读取文件的 byte[] 
        byte[] bytes = new byte[fileStream.Length];
        await fileStream.ReadAsync(bytes, 0, bytes.Length);
        // 把 byte[] 转换成 Stream 
        Stream stream = new MemoryStream(bytes);
        return stream;
    }
    /// <summary>
    /// 从文件读取 Stream 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Stream ReadFile(string filePath)
    {
        // 打开文件 
        using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        // 读取文件的 byte[] 
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        // 把 byte[] 转换成 Stream 
        Stream stream = new MemoryStream(bytes);
        return stream;
    }
    #endregion
    /// <summary>
    /// 拷贝文件，如果目标文件存在则覆盖
    /// </summary>
    /// <param name="orignFile">原始文件</param>
    /// <param name="newFile">新文件路径</param>
    public static void Copy(string orignFile, string newFile)
    {
        string? dir = System.IO.Path.GetDirectoryName(newFile);
        if (!string.IsNullOrWhiteSpace(dir) && !System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
        System.IO.File.Copy(orignFile, newFile, true);
    }

    /// <summary>
    /// 判断文件编码
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>编码</returns>
    public static System.Text.Encoding GetEncodeType(string path)
    {
        System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
        Byte[] buffer = br.ReadBytes(2);
        if (buffer[0] >= 0xEF)
        {
            if (buffer[0] == 0xEF && buffer[1] == 0xBB)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
            {
                return System.Text.Encoding.Unicode;
            }
            else
            {
                return System.Text.Encoding.Default;  //System.Text.Encoding.Default是指操作系统的当前 ANSI 代码页的编码
            }
        }
        else
        {
            return System.Text.Encoding.Default;
        }
    }
    /// <summary>
    /// 获得文件真实格式
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    /// <remarks>https://blog.nowcoder.net/n/ffe451b014c5428295e1d114424de83a</remarks>
    public static OfficeFileClassEnum GetFileHeader(string path) {
        // 初始化文件头十六进制字符串和返回的文件格式类型
        var byteArray = new byte[8];   //设置二进制数组
        System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
        r.Read(byteArray, 0, 8);
        string fileCode = ConvertHelper.ByteToHexStr(byteArray);
        //如果顺利取出前8个字符生成的十六进制字符串

        //对文件头进行简单判断
        OfficeFileClassEnum fileClassEnum = OfficeFileClassEnum.Unknown;

        // 先判断前2位
        var fileCodeHeaderType = fileCode.Substring(0, 4);
        fileClassEnum = fileCodeHeaderType switch {
            "FFD8" => OfficeFileClassEnum.JPG,
            "4749" => OfficeFileClassEnum.GIF,
            "424D" => OfficeFileClassEnum.BMP,
            "8950" => OfficeFileClassEnum.PNG,
            "5261" => OfficeFileClassEnum.RAR,
            "EFBB" => OfficeFileClassEnum.ASPX,
            "7573" => OfficeFileClassEnum.CS,
            "7769" => OfficeFileClassEnum.JS,
            "D2BB" => OfficeFileClassEnum.TXT,
            "4065" => OfficeFileClassEnum.BAT,
            "6438" => OfficeFileClassEnum.BTSEED,
            "3842" => OfficeFileClassEnum.PSD,
            "2550" => OfficeFileClassEnum.PDF,
            "4954" => OfficeFileClassEnum.CHM,
            "4669" => OfficeFileClassEnum.LOG,
            "5245" => OfficeFileClassEnum.REG,
            "3F5F" => OfficeFileClassEnum.HLP,
            "3C3F" => OfficeFileClassEnum.XML,
            "3C21" => OfficeFileClassEnum.HTML,
            _ => fileClassEnum
        };
        if (fileCodeHeaderType == "4143") {
            // 判断前3位
            fileCodeHeaderType = fileCode.Substring(0, 6);
            fileClassEnum = fileCodeHeaderType switch {
                "414331" => OfficeFileClassEnum.DWG,
                //"414376" => OfficeFileClassEnum.SLE,
                _ => fileClassEnum
            };
        }
        else if (fileCodeHeaderType == "4D5A") {
            // 判断前3位
            fileCodeHeaderType = fileCode.Substring(0, 6);
            fileClassEnum = fileCodeHeaderType switch {
                "4D5AEE" => OfficeFileClassEnum.COM,
                "4D5A78" => OfficeFileClassEnum.EXE,
                "4D5A90" => OfficeFileClassEnum.DLL,
                _ => fileClassEnum
            };
        }


        fileCodeHeaderType = fileCode.Substring(0, 8);//取字符串前8个十六进制位（4个字节位）进行判断
        if (fileCodeHeaderType == "504B0304") {
            fileCodeHeaderType = fileCode.Substring(0, 10);
            fileClassEnum = fileCodeHeaderType switch {
                "504B030406" => OfficeFileClassEnum.ZIP,
                "504B030400" => OfficeFileClassEnum.DOCX,
                "504B03040A" => OfficeFileClassEnum.XLSX,
                _ => fileClassEnum
            };
        }
        else if (fileCodeHeaderType == "D0CF11E0") {
            fileCodeHeaderType = fileCode.Substring(0, 10);
            fileClassEnum = fileCodeHeaderType switch {
                "D0CF11E0A1" => OfficeFileClassEnum.DOC,
                //"D0CF11E0A1" => OfficeFileClassEnum.XLS,
                _ => fileClassEnum
            };
        }

        return fileClassEnum;

    }


    /// <summary>
    /// 文件路径处理
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>去除了特殊字符的文件名的文件路径</returns>
    public static string PathProcess(string filePath)
    {
        string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
        fileName = NameProcess(fileName);
        string extension = System.IO.Path.GetExtension(filePath);
        string? fileDir = System.IO.Path.GetDirectoryName(filePath);
        return System.IO.Path.Combine(fileDir ?? "", fileName + extension);
    }
    /// <summary>
    /// 文件名处理
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>去除了特殊字符的文件名</returns>
    public static string NameProcess(string fileName)
    {
        return fileName.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
    }
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件路径</param>
    private static void Create(string path)
    {
        path = PathProcess(path);
        string? dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        if (!string.IsNullOrWhiteSpace(path) && !File.Exists(path))
        {
            File.Create(path).Close();
        }
    }
    // todo 获取文件的MIME类型
    
    ///// <summary>
    ///// 获取文件的MIME类型
    ///// </summary>
    ///// <param name="fileName">文件名</param>
    ///// <returns>MIME类型</returns>
    //public string GetMimeMapping(string fileName)
    //{
    //    new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);
    //    return contentType ?? "application/octet-stream";
    //}
}
/// <summary>
/// 
/// </summary>
/// <remarks>https://blog.mythsman.com/post/5d301940976abc05b345469f/</remarks>
public enum OfficeFileClassEnum {
    JPG,
    GIF,
    BMP,
    PNG,
    COM,
    EXE,
    DLL,
    RAR,
    ZIP,
    XML,
    HTML,
    ASPX,
    CS,
    JS,
    TXT,
    SQL,
    BAT,
    BTSEED,
    RDP,
    PSD,
    PDF,
    CHM,
    LOG,
    REG,
    HLP,
    DOC,
    XLS,
    DOCX,
    XLSX,
    DWG,
    Unknown
}