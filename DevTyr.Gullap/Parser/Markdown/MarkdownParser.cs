using System;
using System.IO;
using MarkdownSharp;
using DevTyr.Gullap.Parser;

namespace DevTyr.Gullap.Parser.Markdown
{
	internal class MarkdownParser : IParser
	{
		public ParsedFileInfo ParseFile (string filePath)
		{
			if (!File.Exists (filePath))
				throw new FileNotFoundException ("File {0} could not be found.", filePath);

			ParsedFileInfo info = new ParsedFileInfo ();
			info.FileName = filePath;

			string[] lines = File.ReadAllLines (filePath);
			bool endReached = false;

			string content = string.Empty;

			foreach (string line in lines) {
				if (!endReached) {
					ParseMarkdownHeaderLineNew(line, info);
					endReached = IsEndOfMarkdownHeader(line);
				} else if (endReached && string.IsNullOrWhiteSpace (info.Link)) {
					content += line + System.Environment.NewLine;
				} else
					continue;
			}

			if (!string.IsNullOrWhiteSpace (content)) {
				MarkdownOptions markdownOptions = new MarkdownOptions () {
					AutoHyperlink = true
				};

				MarkdownSharp.Markdown markdown = new MarkdownSharp.Markdown (markdownOptions);
				info.ParsedContent = markdown.Transform (content);
			}

			return info;
		}

		private void ParseMarkdownHeaderLineNew (string line, ParsedFileInfo info)
		{
			foreach (var keyValue in ParserKeys.Mappings) {
				if (line.ToUpperInvariant().StartsWith(keyValue.Value.ToUpperInvariant())) {
					var value = line.Replace(keyValue.Value, "").Trim();

					var propertyInfo = info.GetType().GetProperty(keyValue.Key);
					if (propertyInfo.CanWrite) {
						propertyInfo.SetValue(info, value, null);
					}
					break;
				}
			}
		}

		private bool IsEndOfMarkdownHeader(string line) {
			if (line.StartsWith (ParserKeys.InfoEnd))
				return true;
			return false;
		}
	}
}

