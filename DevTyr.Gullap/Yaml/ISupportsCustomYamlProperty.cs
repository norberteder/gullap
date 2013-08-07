using System.Collections.Specialized;

namespace DevTyr.Gullap.Yaml
{
    public interface ISupportsCustomYamlProperty
    {
        NameValueCollection Customs { get; }
    }
}
