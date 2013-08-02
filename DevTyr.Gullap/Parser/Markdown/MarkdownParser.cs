using System;
using System.Collections.Generic;
using MarkdownSharp;

namespace DevTyr.Gullap.Parser.Markdown
{
	public class MarkdownParser : IParser
	{
	    public IEnumerable<string> SupportedFileExtensions
	    {
	        get
	        {
	            yield return "md";
	            yield return "markdown";
	        } 
	    }

		public ParsedFileInfo Parse (string content)
		{
            Guard.NotNullOrEmpty(content, "content");

			var info = new ParsedFileInfo();

		    var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			var endReached = false;

			foreach (var line in lines) {
				if (!endReached) {
					ParseMarkdownHeaderLineNew(line, info);
					endReached = IsEndOfMarkdownHeader(line);
				} else if (string.IsNullOrWhiteSpace (info.Link)) {
					content += line + Environment.NewLine;
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

