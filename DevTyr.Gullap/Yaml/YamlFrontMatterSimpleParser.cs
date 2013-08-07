using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DevTyr.Gullap.Yaml
{
    public class YamlFrontMatterSimpleParser
    {
        const string FrontMatterPattern = @"(---)(\n|(\r\n))((.*)(:)(.*)(\n|(\r\n)))*(---)";

        public T ParseFrontMatterInto<T>(string data)
        {
            if (!HasValidFrontMatter(data))
                throw new YamlFrontMatterParserException("No valid YAML Front Matter");

            var frontMatter = GetFrontMatter(data);

            frontMatter = frontMatter.Replace("---", "");

            var splitted = frontMatter.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var result = Activator.CreateInstance<T>();

            foreach (var line in splitted)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.Length == 0)
                    continue;

                var values = trimmedLine.Split(':');

                if (values.Length != 2) continue;

                var propertyInfo = result.GetType().GetProperty(values[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(result, values[1].Trim(), null);
                }
                else
                {
                    if (typeof(ISupportsCustomYamlProperty).IsAssignableFrom(typeof(T)))
                    {
                        ((ISupportsCustomYamlProperty)result).Customs.Add(values[0].Trim(), values[1].Trim());
                    }
                }
            }

            return result;
        }

        public bool HasValidFrontMatter(string data)
        {
            return Regex.IsMatch(data, FrontMatterPattern);
        }

        public string GetContentExceptFrontMatter(string data)
        {
            var result = Regex.Replace(data, FrontMatterPattern, "");
            if (!string.IsNullOrWhiteSpace(result))
                return result.Trim();
            return null;
        }

        public string GetFrontMatter(string data)
        {
            var match = Regex.Match(data, FrontMatterPattern);
            return match.Success ? match.Value : null;
        }
    }
}
