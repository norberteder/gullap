using System;
using System.ServiceModel.Syndication;
using System.IO;
using DevTyr.Gullap.Parser;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevTyr.Gullap.Rss2Gullap
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Rss2Gullap");

			if (args.Length != 1) {
				Console.WriteLine ("Rss2Gullap [rss url]");
			}

			Uri rssUri = new Uri (args [0]);

			SyndicationFeed feed = RssReceiver.LoadRss (rssUri);

			if (!Directory.Exists ("output"))
				Directory.CreateDirectory ("output");

			List<ParsedFileInfo> infos = new List<ParsedFileInfo> ();

			foreach (var item in feed.Items) {
				ParsedFileInfo info = new ParsedFileInfo ();

				info.Author = item.Authors.Count > 0 ? item.Authors [0].Name : null;
				info.ParsedContent = item.Summary.Text;
				info.Title = item.Title.Text;

				infos.Add (info);
			}

			int count = 1;
			foreach (var info in infos) {
				string filename = Path.Combine("output", count.ToString() + ".markdown");

				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Title: " + info.Title);
				builder.AppendLine("Author: " + info.Author);
				builder.AppendLine(ParserKeys.InfoEnd);
				builder.AppendLine(ConvertToMarkdown(info.ParsedContent));
				// just for comparison
				//builder.AppendLine(ParserKeys.InfoEnd);
				//builder.AppendLine(info.ParsedContent);

				File.WriteAllText(filename, builder.ToString());

				count++;
			}
		}

		private static string ConvertToMarkdown (string html)
		{
			var trimmedHtml = html.Trim ().Replace("&gt;", ">").Replace("&lt;", "<");
			string markdown = string.Empty;

			string lastTag = "";
			string currentTag = "";
			bool isClosingTag = false;
			bool parsingTag = false;
			bool wasNewParagraph = false;
			bool isWithinBlockquote = false;
            bool isWithinLink = false;
            bool isWithinImage = false;
            bool isInnerValue = false;

			foreach (char c in trimmedHtml) {
                if (parsingTag && c == '"')
                {
                    isInnerValue = !isInnerValue;
                }
                if (currentTag.StartsWith("a"))
                    isWithinLink = true;
                if (currentTag.StartsWith("img"))
                    isWithinImage = true;
				if (c == '<' && !isWithinLink && !isWithinImage) {
					// tag starts
					parsingTag = true;
					currentTag = "";
				} else if (c == '/' && parsingTag) {
                    if (!isInnerValue)
					    isClosingTag = true;
                    if (isWithinLink || isWithinImage)
                        currentTag += c;
				} else if (c == '>' && parsingTag) {
					// tag ends
                    if (isWithinLink || isWithinImage)
                        currentTag += ">";
					if (currentTag == "b" || currentTag == "strong")
						markdown += "**";
					if (currentTag == "i")
						markdown += "*";
					if (currentTag == "br") {
						markdown += System.Environment.NewLine;
						wasNewParagraph = true;
					}

					if (!isClosingTag) {
						// do it only when it's the starting tag
						if (currentTag == "ul") {
							markdown += System.Environment.NewLine;
							wasNewParagraph = true;
						}
						if (currentTag == "li")
							markdown += "* ";
						if (currentTag == "h1") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "# ";
						}
						if (currentTag == "h2") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "## ";
						}
						if (currentTag == "h3") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "### ";
						}
						if (currentTag == "h4") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "#### ";
						}
						if (currentTag == "h5") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "##### ";
						}
						if (currentTag == "h6") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							markdown += "###### ";
						}
						if (currentTag == "blockquote") {
							markdown += System.Environment.NewLine + System.Environment.NewLine + "> ";
							isWithinBlockquote = true;
						}
						if (currentTag == "pre" || currentTag == "code") {
							markdown += "\x0060";
						}
					}

					if (isClosingTag) {
						if (currentTag == "p" && !isWithinBlockquote) {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							wasNewParagraph = true;
						}
						if (currentTag == "h1" || currentTag == "h2" || currentTag == "h3" | currentTag == "h4" | currentTag == "h5" | currentTag == "h6") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							wasNewParagraph = true;
						}
						if (currentTag == "li") {
							markdown += System.Environment.NewLine;
							wasNewParagraph = true;
						}
						if (currentTag == "ul") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							wasNewParagraph = true;
						}
						if (currentTag == "blockquote") {
							markdown += System.Environment.NewLine + System.Environment.NewLine;
							wasNewParagraph = true;
							isWithinBlockquote = false;
						}
						if (currentTag == "pre" || currentTag == "code")
							markdown += "\x0060";
                        if (currentTag.StartsWith("a"))
                        {
                            Regex hrefRegex = new Regex("href=\"(.*?)\"", RegexOptions.IgnoreCase);
                            Regex titleRegex = new Regex("title=\"(.*?)\"", RegexOptions.IgnoreCase);
                            Regex valueRegex = new Regex(">(.*?)<", RegexOptions.IgnoreCase);

                            var hrefMatch = hrefRegex.Match(currentTag);
                            var titleMatch = titleRegex.Match(currentTag);
                            var valueMatch = valueRegex.Match(currentTag);

                            var value = string.Format("[{0}]({1} \"{2}\")", valueMatch.Groups[1].Value, hrefMatch.Groups[1].Value, titleMatch.Groups[1].Value);
                            markdown += value;
                            isWithinLink = false;
                        }
                        if (currentTag.StartsWith("img"))
                        {
                            Regex srcRegex = new Regex("src=\"(.*?)\"", RegexOptions.IgnoreCase);
                            Regex titleRegex = new Regex("title=\"(.*?)\"", RegexOptions.IgnoreCase);
                            Regex altRegex = new Regex("alt=\"(.*?)\"", RegexOptions.IgnoreCase);

                            var srcMatch = srcRegex.Match(currentTag);
                            var titleMatch = titleRegex.Match(currentTag);
                            var altMatch = altRegex.Match(currentTag);

                            var value = string.Format("![{0}]({1} \"{2}\")", titleMatch.Groups[1].Value, srcMatch.Groups[1].Value, altMatch.Groups[1].Value);
                            markdown += value;
                            isWithinImage = false;
                        }

                        if (!isWithinLink && !isWithinImage)
                        {
                            lastTag = currentTag;
                            currentTag = "";
                        }
					}

                    if (!isWithinLink && !isWithinImage)
                    {
                        parsingTag = false;
                        isClosingTag = false;
                    }
				} else {
                    if (parsingTag && (isWithinLink || isWithinImage || c != ' '))
						currentTag += c;
					else {
						if (!wasNewParagraph)
							markdown += c;
						if (wasNewParagraph && c != ' ') {
							markdown += c;
							wasNewParagraph = false;
						}
					}
				}

			}

			return markdown;
		}
	}
}
