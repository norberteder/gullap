using System.IO;

namespace DevTyr.Gullap
{
	internal class SiteGenerator
	{
		public void Generate (SitePaths paths)
		{
			if (!Directory.Exists(paths.SitePath))
				Directory.CreateDirectory(paths.SitePath);

			if (!Directory.Exists(paths.AssetsPath))
				Directory.CreateDirectory(paths.AssetsPath);

			if (!Directory.Exists(paths.OutputPath))
				Directory.CreateDirectory(paths.OutputPath);

			if (!Directory.Exists(paths.PagesPath))
				Directory.CreateDirectory(paths.PagesPath);

			if (!Directory.Exists(paths.TemplatePath))
				Directory.CreateDirectory(paths.TemplatePath);
		}
	}
}

