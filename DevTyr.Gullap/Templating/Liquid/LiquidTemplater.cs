using System;
using System.IO;

namespace DevTyr.Gullap.Templating.Liquid
{
	public class LiquidTemplater : ITemplater
	{
		public string Transform (string template, object content)
		{
			var parsedTemplate = DotLiquid.Template.Parse(template);
			return parsedTemplate.Render();
		}
	}
}

