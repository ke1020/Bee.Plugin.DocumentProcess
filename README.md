# [“日理万机” 工具箱](https://gitee.com/xkpro/bee) 文档处理插件

插件功能基于 `pandoc` 命令行工具封装，与 `pandoc` 的输入、输出格式一致！

如果您需要将文件从一种标记格式转换为另一种标记格式，`pandoc` 是您的瑞士军刀。`Pandoc` 可以在以下格式之间进行转换：

请注意，箭头表示转换方向，双向箭头（↔︎）表示可以相互转换，单向箭头（→ 或 ←）表示只能从一个方向转换到另一个。

| 类别 | 格式 |
| --- | --- |
| 轻量级标记格式 | ↔︎ Markdown (包括 CommonMark 和 GitHub 风格的 Markdown) <br> ↔︎ reStructuredText <br> → AsciiDoc <br> ↔︎ Emacs Org-Mode <br> ↔︎ Emacs Muse <br> ↔︎ Textile <br> → Markua <br> ← txt2tags <br> ↔︎ djot |
| HTML 格式 | ↔︎ (X)HTML 4 <br> ↔︎ HTML5 <br> → 分块 HTML |
| 电子书 | ↔︎ EPUB 版本 2 或 3 <br> ↔︎ FictionBook2 |
| 文档格式 | → GNU TexInfo <br> ↔︎ Haddock 标记 |
| Roff 格式 | ↔︎ roff man <br> → roff ms <br> ← [mdoc man] |
| TeX 格式 | ↔︎ LaTeX <br> → ConTeXt |
| XML 格式 | ↔︎ DocBook 版本 4 或 5 <br> ↔︎ JATS <br> ← BITS <br> → TEI Simple <br> → OpenDocument XML |
| 大纲格式 | ↔︎ OPML |
| 引文格式 | ↔︎ BibTeX <br> ↔︎ BibLaTeX <br> ↔︎ CSL JSON <br> ↔︎ CSL YAML <br> ← RIS <br> ← EndNote XML |
| 文字处理格式 | ↔︎ Microsoft Word docx <br> ↔︎ 富文本格式 RTF <br> ↔︎ OpenOffice/LibreOffice ODT |
| 交互式笔记本格式 | ↔︎ Jupyter 笔记本 (ipynb) |
| 页面布局格式 | → InDesign ICML <br> ↔︎ Typst |
| 维基标记格式 | ↔︎ MediaWiki 标记 <br> ↔︎ DokuWiki 标记 <br> ← TikiWiki 标记 <br> ← TWiki 标记 <br> ← Vimwiki 标记 <br> → XWiki 标记 <br> → ZimWiki 标记 <br> ↔︎ Jira 维基标记 <br> ← Creole |
| 幻灯片格式 | → LaTeX Beamer <br> → Microsoft PowerPoint <br> → Slidy <br> → reveal.js <br> → Slideous <br> → S5 <br> → DZSlides |
| 数据格式 | ← CSV 表格 <br> ← TSV 表格 |
| 终端输出 | → ANSI 格式的文本 |
| 自定义格式 | ↔︎ 使用 Lua 编写的自定义读取器和写入器 |
| PDF | → 通过 pdflatex, lualatex, xelatex, latexmk, tectonic, wkhtmltopdf, weasyprint, prince, pagedjs-cli, context, 或 pdfroff |

## 运行截图

![](./docs/images/document-convert.png)


## 使用方法

将插件解包后放置在主程序 `Plugins` 根目录下