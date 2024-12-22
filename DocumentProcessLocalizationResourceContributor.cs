

using Bee.Base.Abstractions.Localization;
using Bee.Base.Models;

using Microsoft.Extensions.Options;

namespace Bee.Plugin.DocumentProcess;

public class DocumentProcessLocalizationResourceContributor(IOptions<AppSettings> appSettings) : 
    LocalizationResourceContributorBase(appSettings, DocumentProcessConsts.PluginName)
{
}