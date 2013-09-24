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
		    var workspaceFiles = workspaceInfo.GetContents();

		    var contentsToParse = workspaceFiles.FilesToParse;

		    FillCategoryPages(contentsToParse);

            Trace.TraceInformation("Parsing contents for {0} files", contentsToParse.Count);

		    ParseContents(contentsToParse);

            Trace.TraceInformation("Generating template data");
            Trace.TraceInformation("Found {0} pages", contentsToParse.Count);

			foreach (var content in contentsToParse) 
            {
                dynamic metadata = content.Page != null ? ParseTemplateData(contentsToParse, content.Page) : ParseTemplateData(contentsToParse, content.Post);

				Export (content, metadata);
                Trace.TraceInformation("Exported {0}", content.FileName);
			}

		    foreach (var file in workspaceFiles.FilesNotToParse)
		    {
		        Copy(file);
		    }
		}

        private void FillCategoryPages(List<MetaContent> metaContents)
        {
            foreach (var content in metaContents)
            {
                if (content.Post != null && !string.IsNullOrWhiteSpace(content.Post.Category))
                {
                    content.Post.CategoryPosts =
                        metaContents.Where(item =>  item.Post != null && !string.IsNullOrWhiteSpace(item.Post.Category) && item.Post.Category == content.Post.Category)
                            .Select(item => item.Post)
                            .ToList();
                }
                if (content.Page != null && !string.IsNullOrWhiteSpace(content.Page.Category))
                {
                    content.Page.CategoryPages =
                        metaContents.Where(item => item.Page != null && !string.IsNullOrWhiteSpace(item.Page.Category) && item.Page.Category == content.Page.Category)
                            .Select(item => item.Page)
                            .ToList();
                }
            }
        }

        private dynamic ParseTemplateData(List<MetaContent> metaContents, ContentBase currentContent)
        {
            dynamic metadata = new
            {
                site = new
                {
                    config = Options.SiteConfiguration,
                    time = DateTime.Now,
                    pages = metaContents.Where(content => content.Page != null && !content.Page.Draft).Select(content => content.Page).ToArray(),
                    posts = metaContents.Where(content => content.Post != null && !content.Post.Draft).Select(content => content.Post).ToArray(),
                    pageCategories = metaContents.Where(content => content.Page != null && !string.IsNullOrWhiteSpace(content.Page.Category)).Select(t => new  { t.Page.Category, t.Page }).ToDictionary(arg => arg, arg => arg),
                    postCategories = metaContents.Where(content => content.Post != null && !string.IsNullOrWhiteSpace(content.Post.Category)).Select(t => new { t.Post.Category, t.Post}).ToDictionary(arg => arg, arg => arg)
                },
                current = currentContent
            };

            return metadata;
        }

        private void ParseContents(IEnumerable<MetaContent> metaContents)
        {
            foreach (var content in metaContents)
            {
                Trace.TraceInformation("Parsing content for " + content.FileName);
                if (content.Page != null)
                {
                    content.Page.Content = internalParser.Parse(content.Page.Content);
                }
                if (content.Post != null)
                {
                    content.Post.Content = internalParser.Parse(content.Post.Content);
                }
            }
        }

		private void Export (MetaContent metaContent, dynamic metadata)
		{
		    var targetPath = metaContent.GetTargetFileName(Paths);

		    if (!metaContent.HasValidTemplate())
		    {
                Trace.TraceWarning("No template given for file {0}", metaContent.FileName);
		        return;
		    }

            FileSystem.EnsureDirectory(targetPath);

		    var result = internalTemplater.Transform (Paths.TemplatePath, metaContent.GetTemplate(), metadata);

		    File.WriteAllText (targetPath, result);
		}

	    private void Copy(string file)
	    {
            var targetDirectory = Path.GetDirectoryName(file.Replace(Paths.PagesPath, Paths.OutputPath).Replace(Paths.PostsPath, Paths.OutputPath));

	        FileSystem.EnsureDirectory(targetDirectory);

	        var targetFileName = Path.GetFileName(file);
            File.Copy(file, Path.Combine(targetDirectory, targetFileName));
	    }
	}
}

