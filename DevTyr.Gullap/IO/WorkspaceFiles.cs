using System.Collections.Generic;
using DevTyr.Gullap.Model;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceFiles
    {
        private readonly List<MetaPage> filesToParse = new List<MetaPage>();
        private readonly List<string> filesNotToParse = new List<string>();

        public List<MetaPage> FilesToParse 
        {
            get
            {
                return filesToParse;
            } 
        }

        public List<string> FilesNotToParse
        {
            get
            {
                return filesNotToParse;
            }
        }
    }
}
