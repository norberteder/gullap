using System.IO;

namespace DevTyr.Gullap.Templating.Nustache
{
	public class NustacheTemplater : ITemplater
	{
		public string Transform (string templatePath, string template, object content)
		{
			if (string.IsNullOrWhiteSpace (templatePath) || !Directory.Exists (templatePath))
				throw new DirectoryNotFoundException ("Could not find template directory " + templatePath);
			
			string mainTemplate = GetTemplate(templatePath, template);
			return global::Nustache.Core.Render.FileToString(mainTemplate, content);
		}

		private string GetTemplate (string templatePath, string templateName)
		{
			string mainTemplate = Path.Combine (templatePath, templateName);
			
			if (!File.Exists (mainTemplate)) {
				// fallback
				mainTemplate += ".template";
				if (!File.Exists (mainTemplate)) {
					throw new FileNotFoundException ("Could not find template " + mainTemplate);
				}
			}
			return mainTemplate;
		}
	}
}

