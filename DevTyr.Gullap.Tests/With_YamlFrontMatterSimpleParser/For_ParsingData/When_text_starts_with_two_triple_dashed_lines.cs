using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DevTyr.Gullap.Yaml;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_YamlFrontMatterSimpleParser.For_ParsingData
{
	[TestFixture]
	public class When_text_starts_with_two_triple_dashed_lines
	{
		readonly string data = "---" + Environment.NewLine + "---";
		readonly YamlFrontMatterSimpleParser parser = new YamlFrontMatterSimpleParser();

		[Test]
		public void Should_recognize_as_valid_front_matter ()
		{
			parser.HasValidFrontMatter (data)
				.Should ()
				.BeTrue ();
		}
	}
    
}
