using System.IO;
using DevTyr.Gullap.Model;
using YamlDotNet.RepresentationModel.Serialization;

namespace DevTyr.Gullap
{
    public static class SiteConfigurationParser
    {
        public static SiteConfiguration LoadSiteConfiguration(string file)
        {
            if (!File.Exists(file))
                return null;

            var deserializer = new Deserializer();

            using (var reader = new StreamReader(file))
            {
                return deserializer.Deserialize<SiteConfiguration>(reader);
            }
        }
    }
}
