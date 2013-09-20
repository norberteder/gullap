using System;
using System.Dynamic;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DevTyr.Gullap.IO;
using DevTyr.Gullap.Model;
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
				throw new ArgumentNullException("parser");

			internalParser = parser;
		}

		public void SetTemplater (ITemplater templater)
		{
			if (templater == null)
				throw new ArgumentNullException("templater");

			internalTemplater = templater;
		}

		public void InitializeSite ()
		{
			var generator = new SiteGenerator();
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
			var files = Directory.GetFiles(Paths.OutputPath, "*.*", SearchOption.AllDirectories);
			foreach(var file in files)
				File.Delete(file);
		}

		private void CopyAssets ()
		{
			DirectoryExtensions.DirectoryCopy(Paths.AssetsPath, Paths.OutputPath, true);
		}

		private ConverterResult ConvertAllInternal ()
		{
		    var workspaceInfo = new WorkspaceInfo(Paths);
		    var pages = workspaceInfo.GetPages().ToList();

		    FillCategoryPages(pages);

            Console.WriteLine("Parsing contents for {0} files", pages.Count);

		    ParseContents(pages);

            Console.WriteLine("Generating template data");
            Console.WriteLine("Found {0} pages", pages.Count);

			var successMessages = new List<string>();
			
			foreach (var page in pages) 
            {
                dynamic metadata = ParseTemplateData(pages, page.Page);

				Export (page, metadata);
				successMessages.Add("Exported " + page.FileName);
			}
			return new ConverterResult(false, null, successMessages);
		}

        private void FillCategoryPages(List<MetaPage> pages)
        {
            foreach (var page in pages)
            {
                if (!string.IsNullOrWhiteSpace(page.Page.Category))
                {
                    page.Page.CategoryPages = pages.Where(item => item.Page.Category == page.Page.Category).Select(item => item.Page).ToList();
                }
            }
        }

        private dynamic ParseTemplateData(IEnumerable<MetaPage> metaPages, Page currentPage)
        {
            dynamic metadata = new
            {
                site = new
                {
                    config = Options.SiteConfiguration,
                    time = DateTime.Now,
                    pages = metaPages.Where(page => !page.Page.Draft).Select(page => page.Page).ToArray(),
                    categories = metaPages.Where(page => !string.IsNullOrWhiteSpace(page.Page.Category)).Select(t => new  { t.Page.Category, t.Page }).ToDictionary(arg => arg, arg => arg)
                },
                current = currentPage
            };

            return metadata;
        }

        private void ParseContents(IEnumerable<MetaPage> metaPages)
        {
            foreach (var page in metaPages)
            {
                Console.WriteLine("Parsing content for " + page.FileName);
                page.Page.Content = internalParser.Parse(page.Page.Content);
            }
        }

		private void EnsureTargetPath (string targetPath)
		{
		    var directory = Path.GetDirectoryName(targetPath);

			if (!Directory.Exists(directory)) 
			{
				Directory.CreateDirectory(directory);
			}
		}

		private void Export (MetaPage page, dynamic metadata)
		{
		    var targetPath = page.GetTargetFileName(Paths);

		    if (string.IsNullOrWhiteSpace(page.Page.Template))
		    {
		        Console.WriteLine("No template given for file {0}", page.FileName);
		        return;
		    }

		    EnsureTargetPath (targetPath);

		    var result = internalTemplater.Transform (Paths.TemplatePath, page.Page.Template, metadata);

		    File.WriteAllText (targetPath, result);
		}
	}
}

