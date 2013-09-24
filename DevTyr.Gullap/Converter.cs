using System;
using System.Diagnostics;
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

		public void ConvertAll ()
		{
            CleanOutput();
            CopyAssets();
            ConvertAllInternal();
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

		private void ConvertAllInternal ()
		{
		    var workspaceInfo = new WorkspaceInfo(Paths);
		    var workspaceFiles = workspaceInfo.GetPages();

		    var pagesToParse = workspaceFiles.FilesToParse;

		    FillCategoryPages(pagesToParse);

            Trace.TraceInformation("Parsing contents for {0} files", pagesToParse.Count);

		    ParseContents(pagesToParse);

            Trace.TraceInformation("Generating template data");
            Trace.TraceInformation("Found {0} pages", pagesToParse.Count);

			foreach (var page in pagesToParse) 
            {
                dynamic metadata = ParseTemplateData(pagesToParse, page.Page);

				Export (page, metadata);
                Trace.TraceInformation("Exported {0}", page.FileName);
			}

		    foreach (var file in workspaceFiles.FilesNotToParse)
		    {
		        Copy(file);
		    }
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

        private dynamic ParseTemplateData(List<MetaPage> metaPages, Page currentPage)
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
                Trace.TraceInformation("Parsing content for " + page.FileName);
                page.Page.Content = internalParser.Parse(page.Page.Content);
            }
        }

		private void EnsureTargetPath (string targetPath)
		{
		    var directory = Path.GetDirectoryName(targetPath);

            FileSystem.EnsureDirectory(directory);
		}

		private void Export (MetaPage page, dynamic metadata)
		{
		    var targetPath = page.GetTargetFileName(Paths);

		    if (string.IsNullOrWhiteSpace(page.Page.Template))
		    {
                Trace.TraceWarning("No template given for file {0}", page.FileName);
		        return;
		    }

		    EnsureTargetPath (targetPath);

		    var result = internalTemplater.Transform (Paths.TemplatePath, page.Page.Template, metadata);

		    File.WriteAllText (targetPath, result);
		}

	    private void Copy(string file)
	    {
            var targetDirectory = Path.GetDirectoryName(file.Replace(Paths.PagesPath, Paths.OutputPath));

	        FileSystem.EnsureDirectory(targetDirectory);

	        var targetFileName = Path.GetFileName(file);
            File.Copy(file, Path.Combine(targetDirectory, targetFileName));
	    }
	}
}

