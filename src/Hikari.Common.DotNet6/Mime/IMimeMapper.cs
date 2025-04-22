namespace Hikari.Common.Mime;

/// <summary>
/// Mime
/// </summary>
public interface IMimeMapper
{
    /// <summary>
    /// 扩展Mime
    /// </summary>
    /// <param name="extensions">扩展mime映射规则的标准列表</param>
    /// <returns></returns>
    IMimeMapper Extend(params MimeMappingItem[]? extensions);

    /// <summary>
    /// 根据扩展名获取mime type
    /// </summary>
    /// <param name="fileExtension">文件扩展名</param>
    /// <returns>mime类型</returns>
    string GetMimeFromExtension(string? fileExtension);

    /// <summary>
    /// 返回特定Content-Type的文件扩展名，如果未找到任何对应关系，则返回空值
    /// </summary>
    /// <param name="mime">mime类型</param>
    /// <returns>文件扩展名</returns>
    string GetExtensionFromMime(string? mime);
    /// <summary>
    /// 根据路径获取Mime Type
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>mime类型</returns>
    string GetMimeFromPath(string? filePath);
}
