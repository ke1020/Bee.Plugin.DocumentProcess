

using Bee.Base.ViewModels;
using Bee.Plugin.DocumentProcess.Models;

using Ke.DocumentProcess.Abstrations;
using Ke.DocumentProcess.Pandoc.Models;

namespace Bee.Plugin.DocumentProcess.ViewModels;

public partial class DocumentConvertViewModel(TaskListViewModel<DocumentConvertArguments> taskList,
    PandocDocumentProcessOptions pandocDocumentProcessOptions, 
    IDocumentConverter documentConverter) : 
    DocumentProcessViewModelBase<DocumentConvertArguments>(taskList, pandocDocumentProcessOptions, documentConverter)
{

}