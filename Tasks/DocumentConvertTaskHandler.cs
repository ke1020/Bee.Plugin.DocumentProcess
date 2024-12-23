using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models.Tasks;
using Bee.Plugin.DocumentProcess.Models;

using Ke.DocumentProcess.Abstrations;
using Ke.DocumentProcess.Models.Exceptions;
using Ke.DocumentProcess.Pandoc;
using Ke.DocumentProcess.Pandoc.Models;

namespace Bee.Plugin.DocumentProcess.Tasks;

public class DocumentConvertTaskHandler(IDocumentConverter documentConverter,
    PandocDocumentProcessOptions pandocDocumentProcessOptions) :
    ITaskHandler<DocumentConvertArguments>
{
    private readonly IDocumentConverter _documentConverter = documentConverter;
    private readonly PandocDocumentProcessOptions _pandocDocumentProcessOptions = pandocDocumentProcessOptions;

    public async Task<bool> ExecuteAsync(TaskItem taskItem,
        DocumentConvertArguments? argments,
        Action<double> progressCallback,
        CancellationToken cancellationToken = default)
    {
        if (argments == null)
        {
            throw new ArgumentNullException(nameof(argments));
        }

        // 是否显示指定了输入格式
        string inputFormat;
        if (string.IsNullOrWhiteSpace(argments.InputFormat))
        {
            var ext = Path.GetExtension(taskItem.FileName);
            var exists = _documentConverter.AvailableInputFormats.Values
                .Any(x => x.Equals(ext, StringComparison.OrdinalIgnoreCase))
                ;
            if (!exists)
            {
                throw new NotSupportedInputFormatException(ext);
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

        if (argments.OutputFormat == null)
        {
            throw new NotSupportedOutputFormatException();
        }

        // 输出扩展名
        if (!_documentConverter.AvailableOutputFormats.TryGetValue(argments.OutputFormat, out var outputExtension))
        {
            throw new NotSupportedOutputFormatException(argments.OutputFormat);
        }

        // 输入文件是否 url 地址
        bool isUrl = taskItem.FileName.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            taskItem.FileName.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ;

        var fileName = Path.GetFileNameWithoutExtension(taskItem.FileName);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Guid.NewGuid().ToString();
        }

        var args = new PandocArgumentsBuilder(taskItem.FileName, Path.Combine(argments.OutputDirectory, $"{fileName}{outputExtension}"))
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
        if (argments.OutputFormat.Equals("pdf", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(_pandocDocumentProcessOptions.PdfEnginePath))
            {
                throw new NotSupportedPdfEngineException();
            }

            args = args.SetPdfEngine(_pandocDocumentProcessOptions.PdfEnginePath)
                // 未调试成功
                //.SetTemplate(@"C:\Users\ke\dev\proj\avalonia\Bee.Plugin.DocumentProcess\Configs\pdf.tex")
                ;
        }

        // 忽略警告
        args = args.EnableQuiet();

        // 执行转换
        var result = await _documentConverter.ConvertAsync(args.Build(), cancellationToken);
        progressCallback(100);
        return result.IsSuccess;
    }
}