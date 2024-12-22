using Bee.Base.Abstractions.Navigation;
using Bee.Plugin.DocumentProcess.ViewModels;

namespace Bee.Plugin.DocumentProcess.Navigation.Commands;

/// <summary>
/// 导航命令
/// </summary>
/// <param name="vm"></param>
public class DocumentConvertNavigationCommand(IndexViewModel vm) : 
    NavigationCommandBase<IndexViewModel>("DocumentConvert", vm)
{
}
