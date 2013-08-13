using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DevTyr.Gullap.Yaml;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_YamlFrontMatterSimpleParser.Acceptance_Criteria
{
    [TestFixture]
	public class Case_of_simple_front_matter
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
        public void Can_parse_content()
        {
            var content = parser.GetContentExceptFrontMatter(sample.Text);

            content
				.Should ()
				.BeEquivalentTo (sample.Content);
        }

		[Test]
		public void Can_parse_title()
		{
			var frontMatter = parser.ParseFrontMatterInto<SampleYamlFrontMatter>(sample.Text);

			frontMatter.Title
				.Should ()
				.BeEquivalentTo (sample.Title);
		}

		private class SampleYamlFrontMatter 
		{
			public string Text { get; set; }
			public string Title { get; set; }
			public string Content { get; set; }
		}
	}
}
