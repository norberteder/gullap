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
	public class When_front_matter_is_parsed_into_abstract_data_type
	{
        readonly YamlFrontMatterSimpleParser parser = new YamlFrontMatterSimpleParser ();
		readonly SampleYamlFrontMatter sample = new SampleYamlFrontMatter {
			Text = 
				"---" + Environment.NewLine +
				"title: Test" + Environment.NewLine +
				"author: Norbert Eder" + Environment.NewLine +
				"metadescription: this is a description" + Environment.NewLine +
				"---" + Environment.NewLine +
				" " + Environment.NewLine +
				"This is the content",
			Title = "Test", 
			Author = "Norbert Eder",
			Metadescription = "this is a description",
			Content = "This is the content"
		};

        [Test]
        public void Should_parse_title_into_type_with_title_property ()
        {
            var testPage = parser.ParseFrontMatterInto<TestPage> (sample.Text);

            testPage.Title
				.Should ()
				.BeEquivalentTo (sample.Title);
        }

        [Test]
        public void Should_parse_author_into_type_with_author_property ()
        {
            var testPage = parser.ParseFrontMatterInto<TestPageWithAuthorOnly> (sample.Text);

            testPage.Author
				.Should ()
				.BeEquivalentTo (sample.Author);
        }

        [Test]
        public void Should_be_possible_to_retrieve_custom_properties()
        {
            var withCustoms = parser.ParseFrontMatterInto<TestPageCustomEnabled>(sample.Text);

            withCustoms.Customs["metadescription"]
				.ShouldBeEquivalentTo (sample.Metadescription);
        }

        [Test]
        public void Should_work_if_attribute_value_is_empty()
        {
            var sampletext = "---" + Environment.NewLine +
                             "title:" + Environment.NewLine +
                             "author:Norbert Eder" + Environment.NewLine +
                             "---";

            var testPage = parser.ParseFrontMatterInto<TestPage> (sampletext);

			testPage.Title
				.Should ()
				.BeEmpty ();
        }

		private class SampleYamlFrontMatter 
		{
			public string Text { get; set; }
			public string Title { get; set; }
			public string Author { get; set; }
			public string Metadescription { get; set; }
			public string Content { get; set; }
		}

	    private class TestPage
	    {
	        public string Title { get; set; }
			public string Author { get; set; }
	    }

		private class TestPageWithAuthorOnly
		{
			public string Author { get; set; }
		}

	    private class TestPageWithContent : TestPage
	    {
	        public string Content { get; set; }
	    }

	    private class TestPageCustomEnabled : ISupportsCustomYamlProperty
	    {
	        private readonly NameValueCollection customs = new NameValueCollection();

	        public NameValueCollection Customs { get { return customs; } }
	    }
	}
}
