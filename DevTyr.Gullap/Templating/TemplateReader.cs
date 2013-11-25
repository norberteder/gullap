using System;
using System.IO;

namespace DevTyr.Gullap.Templating
{
	public class TemplateReader
	{
		public string ReadTemplate(string templatePath, string template) 
		{
			if (string.IsNullOrWhiteSpace (templatePath) || !Directory.Exists (templatePath))
				throw new DirectoryNotFoundException ("Could not find template directory " + templatePath);
			
			string mainTemplate = GetTemplate(templatePath, template);	
			return mainTemplate;
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

