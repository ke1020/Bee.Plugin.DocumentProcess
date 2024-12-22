

using Bee.Base.ViewModels;
using Bee.Plugin.DocumentProcess.Models;

using Ke.Bee.Localization.Localizer.Abstractions;
using Ke.DocumentProcess.Abstrations;

namespace Bee.Plugin.DocumentProcess.ViewModels;

public partial class DocumentConvertViewModel(TaskListViewModel<DocumentConvertArguments> taskList, ILocalizer l, IDocumentConverter documentConverter) : 
    DocumentProcessViewModelBase<DocumentConvertArguments>(taskList, l, documentConverter)
{

}