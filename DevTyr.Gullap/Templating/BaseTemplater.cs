using System;
using System.IO;

namespace DevTyr.Gullap.Templating
{
	public abstract class BaseTemplater : ITemplater
	{
		public abstract string Transform (string templatePath, string template, object content);
		
		protected string GetTemplate (string templatePath, string templateName)
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

