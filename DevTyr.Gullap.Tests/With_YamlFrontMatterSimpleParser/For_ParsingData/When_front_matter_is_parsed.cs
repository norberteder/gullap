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
    public class When_front_matter_is_parsed
    {
        readonly YamlFrontMatterSimpleParser parser = new YamlFrontMatterSimpleParser ();
		readonly SampleYamlFrontMatter sample = new SampleYamlFrontMatter {
			Text = 
				"---" + Environment.NewLine +
				"title: Test" + Environment.NewLine +
				"---" + Environment.NewLine +
				" " + Environment.NewLine +
				"This is the content",
			Title = "Test", 
			Content = "This is the content"
		};

        [Test]
        public void Should_recognize_as_valid_front_matter ()
        {
			parser.HasValidFrontMatter (sample.Text)
            	.Should ()
				.BeTrue ();
        }

        [Test]
        public void Should_be_possible_to_retrieve_the_content()
        {
            var content = parser.GetContentExceptFrontMatter(sample.Text);

            content
				.Should ()
				.BeEquivalentTo (sample.Content);
        }

		private class SampleYamlFrontMatter 
		{
			public string Text { get; set; }
			public string Title { get; set; }
			public string Content { get; set; }
		}
	}
}
