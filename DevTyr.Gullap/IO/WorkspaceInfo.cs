using System.Diagnostics;
using System.IO;
using System.Linq;
using DevTyr.Gullap.Model;
using DevTyr.Gullap.Yaml;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceInfo
    {
        private readonly ContentParser pageParser = new ContentParser();
        public SitePaths Paths { get; private set; }

        public WorkspaceInfo(SitePaths paths)
        {
            Paths = paths;
        }

        public WorkspaceFiles GetContents()
        {
            EnsureDirectories();

            var workspaceFiles = new WorkspaceFiles();

            var pageFiles = Directory.GetFiles(Paths.PagesPath, "*.*", SearchOption.AllDirectories);
            var postFiles = Directory.GetFiles(Paths.PostsPath, "*.*", SearchOption.AllDirectories);
            var sourceFiles = pageFiles.Concat(postFiles);

            foreach (var file in sourceFiles)
            {
                HandleFile(file, workspaceFiles);
            }

            return workspaceFiles;
        }

        public WorkspaceFiles GetContent(string file)
        {
            EnsureDirectories();

            var workspaceFiles = new WorkspaceFiles();

            var pageFiles = Directory.GetFiles(Paths.PagesPath, "*.*", SearchOption.AllDirectories);
            var postFiles = Directory.GetFiles(Paths.PostsPath, "*.*", SearchOption.AllDirectories);
            var sourceFile = pageFiles.Concat(postFiles).FirstOrDefault(item => item.Contains(file));

            if (string.IsNullOrWhiteSpace(sourceFile))
                return workspaceFiles;

            HandleFile(sourceFile, workspaceFiles);

            return workspaceFiles;
        }

        private void EnsureDirectories()
        {
            if (!Directory.Exists(Paths.PostsPath))
                throw new DirectoryNotFoundException(Paths.PostsPath);

            if (!Directory.Exists(Paths.PagesPath))
                throw new DirectoryNotFoundException(Paths.PagesPath);
        }

        private void HandleFile(string file, WorkspaceFiles workspaceFiles)
        {
            MetaContent metaContent = null;
            try
            {
                metaContent = BuildMetaContent(file);
            }
            catch (InvalidPageException)
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

        private MetaContent BuildMetaContent(string file)
        {
            MetaContent metaContent;
            var content = File.ReadAllText(file);
            var isPage = file.Contains(Paths.PagesPath);

            Trace.TraceInformation("Reading file {0}", file);
            if (isPage)
            {
                var parsed = pageParser.ParsePage(content);
                metaContent = new MetaContent(file) { Page = parsed };
                metaContent.Page.Url = metaContent.GetTargetFileName(Paths).Replace(Paths.OutputPath, "").Replace('\\', '/');
            }
            else
            {
                var parsed = pageParser.ParsePost(content);
                metaContent = new MetaContent(file) { Post = parsed };
                metaContent.Post.Url = metaContent.GetTargetFileName(Paths).Replace(Paths.OutputPath, "").Replace('\\', '/');
            }
            return metaContent;
        }
    }
}
