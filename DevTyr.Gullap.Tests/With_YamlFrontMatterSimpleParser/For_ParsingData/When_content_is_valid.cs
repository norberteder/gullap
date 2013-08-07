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
    public class When_content_is_valid
    {
        private readonly string data = "---" + Environment.NewLine +
                                       "title: Test" + Environment.NewLine +
                                       "author: Norbert Eder" + Environment.NewLine +
                                       "published: false" + Environment.NewLine +
                                       "metadescription: this is a description" + Environment.NewLine +
                                       "---" + Environment.NewLine +
                                       " " + Environment.NewLine +
                                       "This is the content";

        readonly YamlFrontMatterSimpleParser parser = new YamlFrontMatterSimpleParser();

        [Test]
        public void Should_find_a_valid_yaml_front_matter()
        {
            var actual = parser.HasValidFrontMatter(data);
            actual.Should().Be(true);
        }

        [Test]
        public void Should_be_possible_to_parse_into_concrete_type()
        {
            var testPage = parser.ParseFrontMatterInto<TestPage>(data);

            testPage.Should().NotBeNull();
            testPage.Title.Should().BeEquivalentTo("Test");
            testPage.Author.Should().BeEquivalentTo("Norbert Eder");
            testPage.Published.Should().BeEquivalentTo("false");
        }

        [Test]
        public void Should_be_possible_to_retrieve_the_content()
        {
            var content = parser.GetContentExceptFrontMatter(data);

            content.Should().NotBeNullOrEmpty();
            content.Should().BeEquivalentTo("This is the content");
        }

        [Test]
        public void Should_be_possible_to_retrieve_custom_properties()
        {
            var withCustoms = parser.ParseFrontMatterInto<TestPageCustomEnabled>(data);

            withCustoms.Should().NotBeNull();
            withCustoms.Title.Should().BeEquivalentTo("Test");
            withCustoms.Author.Should().BeEquivalentTo("Norbert Eder");
            withCustoms.Published.Should().BeEquivalentTo("false");
            withCustoms.Customs.AllKeys.Should().Contain("metadescription");
            withCustoms.Customs["metadescription"].ShouldBeEquivalentTo("this is a description");
        }

        [Test]
        public void Should_work_if_attribute_value_is_empty()
        {
            var sampleData = "---" + Environment.NewLine +
                             "title:" + Environment.NewLine +
                             "author:Norbert Eder" + Environment.NewLine +
                             "---";

            var testPage = parser.ParseFrontMatterInto<TestPage>(sampleData);

            testPage.Should().NotBeNull();
            testPage.Title.Should().BeEmpty();
            testPage.Author.Should().Be("Norbert Eder");
        }
    }

    public class TestPage
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Published { get; set; }
    }

    public class TestPageWithContent : TestPage
    {
        public string Content { get; set; }
    }

    public class TestPageCustomEnabled : TestPage, ISupportsCustomYamlProperty
    {
        private readonly NameValueCollection customs = new NameValueCollection();

        public NameValueCollection Customs { get { return customs; } }
    }
}
