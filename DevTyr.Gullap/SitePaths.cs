using System.IO;

namespace DevTyr.Gullap
{
	public class SitePaths
	{
		private readonly string sitePath;

		public SitePaths (string sitePath)
		{
			Guard.NotNullOrEmpty(sitePath, "sitePath");

			this.sitePath = sitePath;
		}

		public string SitePath {
			get { return sitePath; }
		}

		public string OutputPath {
			get { return Path.Combine (sitePath, "output"); }
		}

		public string PagesPath {
			get { return Path.Combine (sitePath, "pages"); }
		}

		public string TemplatePath {
			get { return Path.Combine(sitePath, "templates"); }
		}

		public string AssetsPath {
			get { return Path.Combine (sitePath, "assets"); }
		}
	}
}

