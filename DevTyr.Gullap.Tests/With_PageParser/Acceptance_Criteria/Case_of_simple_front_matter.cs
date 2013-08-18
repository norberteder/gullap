using System;
using DevTyr.Gullap.Yaml;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_PageParser.Acceptance_Criteria
{
    [TestFixture]
	public class Case_of_simple_front_matter
    {
        readonly PageParser parser = new PageParser ();
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
            var page = parser.Parse(sample.Text);

            page.Content
				.Should ()
				.BeEquivalentTo (sample.Content);
        }

		[Test]
		public void Can_parse_title()
		{
		    var page = parser.Parse(sample.Text);

			page.Title
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
