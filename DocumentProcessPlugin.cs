
using Bee.Base.Abstractions.Plugin;

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
    }
}
