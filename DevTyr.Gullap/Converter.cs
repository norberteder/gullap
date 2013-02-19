using System;
using MarkdownSharp;
using System.IO;
using System.Collections.Generic;
using DevTyr.Gullap.Menu;
using DevTyr.Gullap.Parser;
using DevTyr.Gullap.Parser.Markdown;
using DevTyr.Gullap.Templating;
using DevTyr.Gullap.Templating.Nustache;
using DevTyr.Gullap.Extensions;

namespace DevTyr.Gullap
{
	public class Converter
	{
		private ConverterOptions Options { get; set; }
		private SitePaths Paths { get; set; }
		private IParser internalParser = new MarkdownParser();
		private ITemplater internalTemplater = new NustacheTemplater();

		public Converter (ConverterOptions options)
		{
			Guard.NotNull(options, "options");
			Guard.NotNullOrEmpty(options.SitePath, "options.SitePath");

			Options = options;

			Paths = new SitePaths(options.SitePath);
		}

		public void SetParser (IParser parser)
		{
			if (parser == null)
				throw new ArgumentException("No valid parser given");

			this.internalParser = parser;
		}

		public void SetTemplater (ITemplater templater)
		{
			if (templater == null)
				throw new ArgumentException("No valid templater given");

			this.internalTemplater = templater;
		}

		public void InitializeSite ()
		{
			SiteGenerator generator = new SiteGenerator();
			generator.Generate(new SitePaths(Options.SitePath));
		}

		public ConverterResult ConvertAll ()
		{
			try {
				CleanOutput();
				CopyAssets();
				return ConvertAllInternal();
			} catch (Exception ex) {
				return new ConverterResult(true, ex.Message, null);
			}
		}

		private void CleanOutput ()
		{
			string[] files = Directory.GetFiles(Paths.OutputPath, "*.*", SearchOption.AllDirectories);
			foreach(var file in files)
				File.Delete(file);
		}

		private void CopyAssets ()
		{
			DirectoryExtensions.DirectoryCopy(Paths.AssetsPath, Paths.OutputPath, true);
		}

		private ConverterResult ConvertAllInternal ()
		{
			string[] sourceFiles = Directory.GetFiles (Paths.PagesPath, "*.*", SearchOption.AllDirectories);
			
			List<ParsedFileInfo> fileInfos = new List<ParsedFileInfo> ();
			
			foreach (var sourceFile in sourceFiles) {
				ParsedFileInfo info = internalParser.ParseFile (sourceFile);
				fileInfos.Add (info);
			}
			
			MenuBuilder menuBuilder = new MenuBuilder ();
			MainMenu mainMenu = menuBuilder.Build (fileInfos);
			
			List<string> successMessages = new List<string>();
			
			foreach (var info in fileInfos) {
				ExportMarkdown (info, Options, mainMenu);
				if (string.IsNullOrWhiteSpace(info.TargetFileName)) {
					successMessages.Add("Handled (not exported) " + Path.GetFileName(info.FileName));
				} else {
					successMessages.Add("Exported " + info.TargetFileName);
				}
			}
			return new ConverterResult(false, null, successMessages);
		}

		private void EnsureTargetPath (string targetPath)
		{
			if (!Directory.Exists (targetPath)) 
			{
				Directory.CreateDirectory(targetPath);
			}
		}

		private void ExportMarkdown (ParsedFileInfo info, ConverterOptions options, MainMenu menu)
		{
			if (info.ShouldBeGenerated) {
				var directoryName = Path.GetDirectoryName (info.TargetFileName);
				if (string.IsNullOrEmpty (directoryName))
					directoryName = System.Environment.CurrentDirectory;
				EnsureTargetPath (directoryName);

				var sidebarItems = menu.GetSidebarItems(info.Sidebar);

				var templateData = new { content = info.ParsedContent, menu = menu, title = info.Title, description = info.Description, author = info.Author, sidebarheader = info.Sidebar, sidebaritems = sidebarItems, keywords = info.Keywords };

				var result = internalTemplater.Transform (Paths.TemplatePath, info.Template, templateData);

				var target = CalculateTargetFileName(info, options);
				File.WriteAllText (target, result);
			}
		}

		private string CalculateTargetFileName (ParsedFileInfo info, ConverterOptions options)
		{
			return Path.Combine(Paths.OutputPath, info.TargetFileName);
		}
	}
}

