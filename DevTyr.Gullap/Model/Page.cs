using System;
using System.Collections.Generic;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Model
{
    public class Page
    {
        private readonly dynamic yaml;

        public Page(DynamicYaml dynamicYaml, string unparsedContent)
        {
            yaml = dynamicYaml;
            Content = unparsedContent;
        }

        public string Title
        {
            get { return yaml.title; }
        }

        public string Description
        {
            get { return yaml.description; }
        }

        public string Category
        {
            get { return yaml.category; }
        }

        public string Author
        {
            get { return yaml.author; }
        }

        public DateTime Date
        {
            get { return DateTime.Parse((string)yaml.date); }
        }

        public List<string> Tags
        {
            get { return yaml.tags; }
        }

        public string Template
        {
            get { return yaml.template; }
        }

        public bool Draft
        {
            get { return yaml.draft; }
        }

        public string Content { get; set; }

        public DynamicYaml DynamicYaml
        {
            get { return yaml; }
        }
    }
}
