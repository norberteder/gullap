using System.IO;

namespace DevTyr.Gullap.Model
{
    public static class MetaContentExtensions
    {
        public static string GetTargetFileName(this MetaContent content, SitePaths paths)
        {
            var isPage = content.Page != null;

            string targetDirectory;

            if (isPage)
            {
                targetDirectory = Path.GetDirectoryName(content.FileName.Replace(paths.PagesPath, paths.OutputPath));
            }
            else
            {
                targetDirectory =
                    Path.GetDirectoryName(content.FileName.Replace(paths.PostsPath,
                        Path.Combine(paths.OutputPath, SitePaths.PostsDirectoryName)));
            }
            var targetFileName = Path.GetFileNameWithoutExtension(content.FileName) + ".html";

            var targetPath = Path.Combine(targetDirectory, targetFileName);
            return targetPath;
        }
    }
}
