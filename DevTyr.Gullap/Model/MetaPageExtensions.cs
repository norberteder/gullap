using System.IO;

namespace DevTyr.Gullap.Model
{
    public static class MetaPageExtensions
    {
        public static string GetTargetFileName(this MetaPage page, SitePaths paths)
        {
            var targetDirectory = Path.GetDirectoryName(page.FileName.Replace(paths.PagesPath, paths.OutputPath));
            var targetFileName = Path.GetFileNameWithoutExtension(page.FileName) + ".html";

            var targetPath = Path.Combine(targetDirectory, targetFileName);
            return targetPath;
        }
    }
}
