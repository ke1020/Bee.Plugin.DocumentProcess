
using Bee.Base.Models.Tasks;

namespace Bee.Plugin.DocumentProcess.Models;

public class DocumentConvertArguments : TaskArgumentBase
{
    /// <summary>
    /// 输出格式
    /// </summary>
    public string? OutputFormat { get; set; }
    /// <summary>
    /// 输入格式。（如果不提供，将尝试从扩展名推断）
    /// </summary>
    public string? InputFormat { get; set; }
    /// <summary>
    /// 是否启用独立文件
    /// </summary>
    public bool EnableStandalone { get; set; } = false;
    /// <summary>
    /// 是否启用请求头
    /// </summary>
    public bool EnableRequestHeader { get; set; } = false;
    /// <summary>
    /// 启用日志
    /// </summary>
    public bool EnableLogFile { get; set; } = false;
    /// <summary>
    /// 提取媒体文件
    /// </summary>
    public bool EnableExtractMedia { get; set; } = false;
    /// <summary>
    /// 嵌入资源
    /// </summary>
    public bool EnableEmbedResources { get; set; } = false;
    /// <summary>
    /// 启用目录
    /// </summary>
    public bool EnableTableOfContents { get; set; } = false;
    /// <summary>
    /// 启用章节编号
    /// </summary>
    public bool EnableNumberSections { get; set; } = false;
    /// <summary>
    /// 请求头
    /// </summary>
    public string? RequestHeader { get; set; }
    /// <summary>
    /// 资源路径
    /// </summary>
    /// public string? ResourcePath { get; set; }
    /// <summary>
    /// 高亮样式
    /// </summary>
    public string? HighlightStyle { get; set; }
}