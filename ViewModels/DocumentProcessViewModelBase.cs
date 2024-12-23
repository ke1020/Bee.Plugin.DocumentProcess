using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models.Tasks;
using Bee.Base.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using Ke.DocumentProcess.Abstrations;
using Ke.DocumentProcess.Pandoc.Models;

namespace Bee.Plugin.DocumentProcess.ViewModels;

public partial class DocumentProcessViewModelBase<T> : ObservableObject where T : TaskArgumentBase, new()
{
    /// <summary>
    /// 是否支持 PDF 输出
    /// </summary>
    private readonly bool _isPdfOutput;
    /// <summary>
    /// 输入格式集合
    /// </summary>
    public List<string> InputFormats => [.. DocumentConverter.AvailableInputFormats.Keys];
    /// <summary>
    /// 输出格式集合
    /// </summary>
    public List<string> OutputFormats => _isPdfOutput ?
        [.. DocumentConverter.AvailableOutputFormats.Keys] :
        [.. DocumentConverter.AvailableOutputFormats.Keys.Where(x => !x.Equals("pdf"))]
        ;
    /// <summary>
    /// 高亮样式集合
    /// </summary>
    public List<string> HighlightStyles => [.. DocumentConverter.AvailableHighlightStyles];
    /// <summary>
    /// 任务列表控件视图模型
    /// </summary>
    public ITaskListViewModel<T> TaskList { get; }
    /// <summary>
    /// 文档转换器
    /// </summary>
    protected IDocumentConverter DocumentConverter { get; }

    public DocumentProcessViewModelBase(TaskListViewModel<T> taskList,
        PandocDocumentProcessOptions pandocDocumentProcessOptions,
        IDocumentConverter documentConverter)
    {
        // 是否支持 PDF 输出
        _isPdfOutput = pandocDocumentProcessOptions.PdfEnginePath != null && File.Exists(pandocDocumentProcessOptions.PdfEnginePath);
        // 文档转换器
        DocumentConverter = documentConverter;
        // 任务列表视图模型
        TaskList = taskList;
        // 初始化任务参数
        TaskList.InitialArguments(DocumentProcessConsts.PluginName);
        // 设置任务列表初始可选择的文件类型
        TaskList.SetInputExtensions(DocumentConverter.AvailableInputFormats.Values.Distinct());
    }
}