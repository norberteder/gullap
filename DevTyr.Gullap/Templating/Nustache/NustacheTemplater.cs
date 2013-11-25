using System.IO;

namespace DevTyr.Gullap.Templating.Nustache
{
	public class NustacheTemplater : ITemplater
	{
		public string Transform (string template, object content)
		{
			return global::Nustache.Core.Render.FileToString(template, content);
		}
	}
}

