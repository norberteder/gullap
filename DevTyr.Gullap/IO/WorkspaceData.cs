using System.Collections.Generic;
using DevTyr.Gullap.Model;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceData
    {
        private readonly List<MetaPage> yamlFiles = new List<MetaPage>();
        private readonly List<string> dotNotParseFiles = new List<string>();

        public List<MetaPage> YamlFiles 
        {
            get
            {
                return yamlFiles;
            } 
        }

        public List<string> DoNotParseFiles
        {
            get
            {
                return dotNotParseFiles;
            }
        }
    }
}
