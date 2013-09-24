using System;
using DevTyr.Gullap.Yaml;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_ContentParser.For_ParsingData
{
    [TestFixture]
    public class When_page_front_matter_is_parsed
    {
        readonly ContentParser parser = new ContentParser ();
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
            var page = parser.ParsePage(sample.Text);
            page.Title
                .Should()
                .NotBeNullOrEmpty();
        }

        [Test]
        public void Should_be_possible_to_retrieve_the_content()
        {
            var page = parser.ParsePage(sample.Text);

            page.Content
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
