using System;
using System.ServiceModel.Syndication;
using System.IO;
using DevTyr.Gullap.Parser;
using System.Collections.Generic;
using System.Text;

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
				builder.AppendLine(ParserKeys.InfoEnd);
				builder.AppendLine(info.ParsedContent);

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

			foreach (char c in trimmedHtml) {
				if (c == '<') {
					// tag starts
					parsingTag = true;
					currentTag = "";
				} else if (c == '/' && parsingTag) {
					isClosingTag = true;
				} else if (c == '>' && parsingTag) {
					// tag ends
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
					}

					if (isClosingTag) {
						lastTag = currentTag;
						currentTag = "";
					}

					parsingTag = false;
					isClosingTag = false;
				} else {
					if (parsingTag && c != ' ')
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
