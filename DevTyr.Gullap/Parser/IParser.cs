using System.Collections.Generic;

namespace DevTyr.Gullap.Parser
{
	public interface IParser
	{
		IEnumerable<string> SupportedFileExtensions { get; }
		string Parse(string content);
	}
}

