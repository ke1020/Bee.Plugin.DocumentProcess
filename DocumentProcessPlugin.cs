
using System.Text.Json;

using Bee.Base.Abstractions.Navigation;
using Bee.Base.Abstractions.Plugin;
using Bee.Base.Abstractions.Tasks;
using Bee.Base.ViewModels;
using Bee.Plugin.DocumentProcess.Models;
using Bee.Plugin.DocumentProcess.Navigation.Commands;
using Bee.Plugin.DocumentProcess.Tasks;
using Bee.Plugin.DocumentProcess.ViewModels;
using Bee.Plugin.DocumentProcess.Views;

using Ke.Bee.Localization.Providers.Abstractions;
using Ke.DocumentProcess.Abstrations;
using Ke.DocumentProcess.Pandoc;
using Ke.DocumentProcess.Pandoc.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Bee.Plugin.DocumentProcess;

/// <summary>
/// 文档处理插件
/// </summary>
/// <param name="serviceProvider"></param>
public class DocumentProcessPlugin(IServiceProvider serviceProvider) : PluginBase(serviceProvider)
{
    public override string PluginName => DocumentProcessConsts.PluginName;

    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IPlugin, DocumentProcessPlugin>();
        services.AddSingleton<ILocalizaitonResourceContributor, DocumentProcessLocalizationResourceContributor>();
        services.AddSingleton<INavigationCommand, DocumentConvertNavigationCommand>();

        // 注入视图模型
        services.AddTransient<IndexViewModel>();
        // 注入文档转换页视图与视图模型
        services.AddTransient<DocumentConvertView>();
        services.AddTransient<DocumentConvertViewModel>();
        // 任务列表视图模型
        services.AddTransient<TaskListViewModel<DocumentConvertArguments>>();
        // 任务处理器
        services.AddTransient<ITaskHandler<DocumentConvertArguments>, DocumentConvertTaskHandler>();

        //注册文档转换器
        services.AddTransient<IDocumentConverter, PandocDocumentConverter>();

        AddPandoc(services);
    }

    private void AddPandoc(IServiceCollection services)
    {
        // 插件根目录
        var pluginRootPath = Path.Combine(AppSettings.PluginPath, PluginName);
        // pandoc 配置文件
        var pandocConfigPath = Path.Combine(pluginRootPath, "Configs", "pandoc.json");
        // 文档转换配置
        PandocDocumentProcessOptions pandocOptions;
        // 优先使用配置文件中指定的配置
        if (File.Exists(pandocConfigPath))
        {
            pandocOptions = JsonSerializer.Deserialize<PandocDocumentProcessOptions>(File.ReadAllBytes(pandocConfigPath))!;
        }
        else
        {
            pandocOptions = new PandocDocumentProcessOptions
            {
                PandocPath = Path.Combine(pluginRootPath, "pandoc-3.6/pandoc.exe"),
                PdfEnginePath = Path.Combine(pluginRootPath, "TinyTeX/bin/windows/xelatex.exe")
            };
        }

        services.AddSingleton(pandocOptions);
    }
}
