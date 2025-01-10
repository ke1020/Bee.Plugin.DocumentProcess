using Bee.Base.Abstractions;
using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models.Tasks;
using Bee.Plugin.DocumentProcess.Models;

using Ke.Bee.Localization.Localizer.Abstractions;
using Ke.DocumentProcess.Abstrations;
using Ke.DocumentProcess.Models.Exceptions;
using Ke.DocumentProcess.Pandoc;
using Ke.DocumentProcess.Pandoc.Models;

using LanguageExt;

using Microsoft.Extensions.Logging;

namespace Bee.Plugin.DocumentProcess.Tasks;

public class DocumentConvertTaskHandler(IDocumentConverter documentConverter,
    PandocDocumentProcessOptions pandocDocumentProcessOptions,
    ICoverHandler coverHandler,
    ILocalizer localizer,
    ILogger<DocumentConvertTaskHandler> logger) :
    TaskHandlerBase<DocumentConvertArguments>(coverHandler)
{
    private readonly IDocumentConverter _documentConverter = documentConverter;
    private readonly PandocDocumentProcessOptions _pandocDocumentProcessOptions = pandocDocumentProcessOptions;
    private readonly ILocalizer _l = localizer;
    private readonly ILogger<DocumentConvertTaskHandler> _logger = logger;

    public override async Task<Fin<Unit>> ExecuteAsync(TaskItem taskItem,
        DocumentConvertArguments argments,
        Action<double> progressCallback,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        progressCallback(1);

        // 是否显示指定了输入格式
        string inputFormat;
        if (string.IsNullOrWhiteSpace(argments.InputFormat))
        {
            var ext = Path.GetExtension(taskItem.Input);
            var exists = _documentConverter.AvailableInputFormats.Values
                .Any(x => x.Equals(ext, StringComparison.OrdinalIgnoreCase))
                ;
            if (!exists)
            {
                return Fin<Unit>.Fail($"{_l["Errors.NotSupported.InputFormat"]} {ext}");
            }
            inputFormat = _documentConverter.AvailableInputFormats
                .FirstOrDefault(x => x.Value.Equals(ext, StringComparison.OrdinalIgnoreCase))
                .Key
                ;
        }
        else
        {
            inputFormat = argments.InputFormat;
        }

        progressCallback(5);

        // 输出扩展名
        if (!_documentConverter.AvailableOutputFormats.TryGetValue(argments.OutputFormat ?? string.Empty, out var outputExtension))
        {
            return Fin<Unit>.Fail($"{_l["Errors.NotSupported.OutputFormat"]} {argments.OutputFormat}");
        }

        // 输入文件是否 url 地址
        bool isUrl = taskItem.Input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            taskItem.Input.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ;

        var fileName = Path.GetFileNameWithoutExtension(taskItem.Input);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Guid.NewGuid().ToString();
        }

        var args = new PandocArgumentsBuilder(taskItem.Input, Path.Combine(argments.OutputDirectory, $"{fileName}{outputExtension}"))
            .SetFrom(inputFormat) // 网址没有扩展名，所以显式指定输入格式
            .SetTo(argments.OutputFormat)
            ;

        // 独立文件模式
        args = argments.EnableStandalone ? args.EnableStandalone() : args;
        // 提取媒体文件
        args = argments.EnableExtractMedia ? args.SetExtractMedia(Path.Combine(argments.OutputDirectory, fileName)) : args;
        // 嵌入资源
        args = argments.EnableEmbedResources ? args.EnableEmbedResources() : args;
        // 输出日志
        args = argments.EnableLogFile ? args.EnableVerbose().EnableLogFile() : args;
        // 表格目录
        args = argments.EnableTableOfContents ? args.EnableTableOfContents() : args;
        // 章节编号
        args = argments.EnableNumberSections ? args.EnableNumberSections() : args;
        // 高亮
        args = !string.IsNullOrWhiteSpace(argments.HighlightStyle) ? args.SetHighlightStyle(argments.HighlightStyle) : args;

        // 输入文件是网页格式时，可以指定请求头
        if (argments.EnableRequestHeader)
        {
            args = args.EnableRequestHeader();
        }

        // pdf 格式
        if (argments.OutputFormat?.Equals("pdf", StringComparison.OrdinalIgnoreCase) == true)
        {
            if (string.IsNullOrWhiteSpace(_pandocDocumentProcessOptions.PdfEnginePath))
            {
                return Fin<Unit>.Fail(new NotSupportedPdfEngineException());
            }

            args = args.SetPdfEngine(_pandocDocumentProcessOptions.PdfEnginePath)
                // 未调试成功
                //.SetTemplate(@"C:\Users\ke\dev\proj\avalonia\Bee.Plugin.DocumentProcess\Configs\pdf.tex")
                ;
        }

        // 忽略警告
        args = args.EnableQuiet();

        progressCallback(10);

        try
        {
            // 执行转换
            await _documentConverter.ConvertAsync(args.Build(), cancellationToken);
            progressCallback(100);
            return Fin<Unit>.Succ(Unit.Default);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Fin<Unit>.Fail($"{_l["Task.Execution.Failed"]}, {fileName}");
        }
    }
}