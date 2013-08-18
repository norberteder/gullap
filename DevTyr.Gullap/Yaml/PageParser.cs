using System;
using DevTyr.Gullap.Model;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Yaml
{
    public class PageParser
    {
        public Page Parse(string content)
        {
            var splitted = content.Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length != 2)
                throw new InvalidPageException();

            var parsed = new DynamicYaml(splitted[0]);

            return new Page(parsed, splitted[1].Trim());

        }
    }
}
