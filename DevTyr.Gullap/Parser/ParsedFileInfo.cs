using System;
using System.IO;

namespace DevTyr.Gullap.Parser
{
	public class ParsedFileInfo
	{
		private string template = "page.template";

		public string FileName { get; set; }
		public string Title { get; set; }
		public string Keywords { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public string Link { get; set; }
		public string Menu { get; set; }
		public string MenuTitle { get; set; }
		public string MenuCategory { get; set; }
		public string Sidebar { get; set; }
		public string SidebarTitle { get; set; }
		public string Date { get; set;}
		public string Directory { get; set;}

		public string ParsedContent { get; set; }

		public string Template {
			get {
				return template;
			} 
			set {
				template = value;
			}
		}

		public string TargetFileName { 
			get {
				if (string.IsNullOrWhiteSpace(FileName) || !ShouldBeGenerated)
					return null;
				
				string sourceExtension = Path.GetExtension (FileName);
				if (sourceExtension.ToLowerInvariant () != ".html") {
					return GetTargetFileName(Path.GetFileName(FileName).Replace (sourceExtension, ".html"));
				} else {
					return GetTargetFileName(FileName);
				}
			}
		}

		private string GetTargetFileName(string fileName) 
		{
			if (!string.IsNullOrWhiteSpace (this.Directory))
				return Path.Combine (this.Directory, fileName);
			return fileName;
		}

		private string GetTargetPath(string fileName) 
		{
			if (!string.IsNullOrWhiteSpace(this.Directory)) 
			{
				return Path.Combine (this.Directory, fileName);
			}
			return fileName;
		}

		public bool ShouldBeGenerated {
			get { return string.IsNullOrWhiteSpace (Link) && !string.IsNullOrWhiteSpace(ParsedContent); }
		}
	}
}