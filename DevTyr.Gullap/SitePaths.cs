using System.IO;

namespace DevTyr.Gullap
{
	public class SitePaths
	{
		private readonly string sitePath;

	    public static readonly string SitePathDirectoryName = "sitePath";
	    public static readonly string OutputDirectoryName = "output";
	    public static readonly string PagesDirectoryName = "pages";
	    public static readonly string PostsDirectoryName = "posts";
	    public static readonly string TemplatesDirectoryName = "templates";
	    public static readonly string AssetsDirectoryName = "assets";

		public SitePaths (string sitePath)
		{
			Guard.NotNullOrEmpty(sitePath, SitePathDirectoryName);

			this.sitePath = sitePath;
		}

		public string SitePath 
        {
			get { return sitePath; }
		}

		public string OutputPath 
        {
			get { return Path.Combine (sitePath, OutputDirectoryName); }
		}

		public string PagesPath 
        {
			get { return Path.Combine (sitePath, PagesDirectoryName); }
		}

	    public string PostsPath
	    {
	        get { return Path.Combine(sitePath, PostsDirectoryName); }
	    }

		public string TemplatePath 
        {
			get { return Path.Combine(sitePath, TemplatesDirectoryName); }
		}

		public string AssetsPath 
        {
			get { return Path.Combine (sitePath, AssetsDirectoryName); }
		}
	}
}

