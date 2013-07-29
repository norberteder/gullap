using System.IO;
using MarkdownSharp;

namespace DevTyr.Gullap.Parser.Markdown
{
	internal class MarkdownParser : IParser
	{
		public ParsedFileInfo ParseFile (string filePath)
		{
			if (!File.Exists (filePath))
				throw new FileNotFoundException ("File {0} could not be found.", filePath);

			var info = new ParsedFileInfo {FileName = filePath};

		    var lines = File.ReadAllLines (filePath);
			var endReached = false;

			var content = string.Empty;

			foreach (var line in lines) {
				if (!endReached) {
					ParseMarkdownHeaderLineNew(line, info);
					endReached = IsEndOfMarkdownHeader(line);
				} else if (string.IsNullOrWhiteSpace (info.Link)) {
					content += line + System.Environment.NewLine;
				}
			}

		    if (string.IsNullOrWhiteSpace(content)) return info;

		    var markdownOptions = new MarkdownOptions
		    {
		        AutoHyperlink = true
		    };

		    var markdown = new MarkdownSharp.Markdown (markdownOptions);
		    info.ParsedContent = markdown.Transform (content);

		    return info;
		}

		private void ParseMarkdownHeaderLineNew (string line, ParsedFileInfo info)
		{
			foreach (var keyValue in ParserKeys.Mappings) {
			    if (!line.ToUpperInvariant().StartsWith(keyValue.Value.ToUpperInvariant())) continue;

			    var value = line.Replace(keyValue.Value, "").Trim();

			    var propertyInfo = info.GetType().GetProperty(keyValue.Key);
			    if (propertyInfo.CanWrite) {
			        propertyInfo.SetValue(info, value, null);
			    }
			    break;
			}
		}

		private bool IsEndOfMarkdownHeader(string line)
		{
		    return line.StartsWith (ParserKeys.InfoEnd);
		}
	}
}

