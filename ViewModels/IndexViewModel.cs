using Bee.Base.Models;
using Bee.Base.ViewModels;
using Bee.Plugin.DocumentProcess.Views;

using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Plugin.DocumentProcess.ViewModels;

public class IndexViewModel : WorkspaceViewModel
{
    protected override List<TabMetadata> TabList =>
    [
        new ("Bee.Plugin.DocumentProcess.Tab.Convert", typeof(DocumentConvertView), typeof(DocumentConvertViewModel)),
    ];

    public IndexViewModel(IServiceProvider serviceProvider, ILocalizer l) : base(serviceProvider, l)
    {
        IsPaneOpen = true;
    }
}