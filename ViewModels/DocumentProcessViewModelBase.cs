using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models.Tasks;
using Bee.Base.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using Ke.Bee.Localization.Localizer.Abstractions;
using Ke.DocumentProcess.Abstrations;

namespace Bee.Plugin.DocumentProcess.ViewModels;

public partial class DocumentProcessViewModelBase<T> : ObservableObject where T : TaskArgumentBase, new()
{
    /// <summary>
    /// 输入格式集合
    /// </summary>
    public List<string> InputFormats => [.. DocumentConverter.AvailableInputFormats.Keys];
    /// <summary>
    /// 输出格式集合
    /// </summary>
    public List<string> OutputFormats => [.. DocumentConverter.AvailableOutputFormats.Keys];
    /// <summary>
    /// 任务列表控件视图模型
    /// </summary>
    public ITaskListViewModel<T> TaskList { get; }
    /// <summary>
    /// 文档转换器
    /// </summary>
    protected IDocumentConverter DocumentConverter { get; }

    public DocumentProcessViewModelBase(TaskListViewModel<T> taskList, ILocalizer l, IDocumentConverter documentConverter)
    {
        DocumentConverter = documentConverter;

        TaskList = taskList;
        // 初始化任务参数
        TaskList.InitialArguments(DocumentProcessConsts.PluginName);
        // 设置任务列表初始可选择的文件类型
        TaskList.SetInputExtensions(DocumentConverter.AvailableInputFormats.Values.Distinct());
        // 设置视图功能说明
        TaskList.SetViewComment(l["Bee.Plugin.DocumentProcess.ViewComment"]);

    }
}