using System.IO;
using DevTyr.Gullap.IO;

namespace DevTyr.Gullap
{
	internal class SiteGenerator
	{
		public void Generate (SitePaths paths)
		{
            FileSystem.EnsureDirectory(paths.SitePath);
            FileSystem.EnsureDirectory(paths.AssetsPath);
            FileSystem.EnsureDirectory(paths.OutputPath);
            FileSystem.EnsureDirectory(paths.PagesPath);
		}
	}
}

