using System;
using System.Runtime.Serialization;

namespace DevTyr.Gullap.Yaml
{
    public class YamlFrontMatterParserException : Exception
    {
        public YamlFrontMatterParserException()
        {
        }

        public YamlFrontMatterParserException(string message)
            : base(message)
        {
        }

        public YamlFrontMatterParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public YamlFrontMatterParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
