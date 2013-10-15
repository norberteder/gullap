using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DevTyr.Gullap.Model;
using DevTyr.Gullap.Yaml;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceInfo
    {
        public SitePaths Paths { get; private set; }

        public WorkspaceInfo(SitePaths paths)
        {
            Paths = paths;
        }

        public WorkspaceFiles GetContents()
        {
            if (!Directory.Exists(Paths.PostsPath))
                throw new DirectoryNotFoundException(Paths.PostsPath);

            if (!Directory.Exists(Paths.PagesPath))
                throw new DirectoryNotFoundException(Paths.PagesPath);

            var workspaceFiles = new WorkspaceFiles();

            var pageFiles = Directory.GetFiles(Paths.PagesPath, "*.*", SearchOption.AllDirectories);
            var postFiles = Directory.GetFiles(Paths.PostsPath, "*.*", SearchOption.AllDirectories);
            var sourceFiles = pageFiles.Concat(postFiles);

            var pageParser = new ContentParser();

            foreach (var file in sourceFiles)
            {
                var content = File.ReadAllText(file);
                var isPage = file.Contains(Paths.PagesPath);

                MetaContent metaContent = null;
                try
                {
                    Trace.TraceInformation("Reading file {0}", file);
                    if (isPage)
                    {
                        var parsed = pageParser.ParsePage(content);
                        metaContent = new MetaContent(file) {Page = parsed};
                        metaContent.Page.Url =
                            metaContent.GetTargetFileName(Paths).Replace(Paths.OutputPath, "").Replace('\\', '/');
                    }
                    else
                    {
                        var parsed = pageParser.ParsePost(content);
                        metaContent = new MetaContent(file) { Post = parsed};
                        metaContent.Post.Url =
                            metaContent.GetTargetFileName(Paths).Replace(Paths.OutputPath, "").Replace('\\', '/');
                    }
                } 
                catch (InvalidPageException ipe)
                {
                    Trace.TraceWarning("Invalid YAML Front Matter for file " + file);
                    var fileExtension = Path.GetExtension(file);
                    if (!string.IsNullOrWhiteSpace(fileExtension) && (fileExtension.ToLowerInvariant() == ".html" || fileExtension.ToLowerInvariant() == ".htm"))
                        workspaceFiles.FilesNotToParse.Add(file);
                }

                if (metaContent != null)
                {
                    workspaceFiles.FilesToParse.Add(metaContent);
                }
            }

            return workspaceFiles;
        }
    }
}
