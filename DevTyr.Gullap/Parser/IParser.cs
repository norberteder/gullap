using System;

namespace DevTyr.Gullap.Parser
{
	public interface IParser
	{
		ParsedFileInfo ParseFile(string filePath);
	}
}

