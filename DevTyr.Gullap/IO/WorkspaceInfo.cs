using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DevTyr.Gullap.Model;
using DevTyr.Gullap.Yaml;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceInfo
    {
        public string WorkspacePath { get; private set; }

        public WorkspaceInfo(string directory)
        {
            WorkspacePath = directory;
        }

        public IEnumerable<MetaPage> GetPages()
        {
            if (!Directory.Exists(WorkspacePath))
                throw new DirectoryNotFoundException();

            var sourceFiles = Directory.GetFiles(WorkspacePath, "*.*", SearchOption.AllDirectories);
            var pageParser = new PageParser();

            foreach (var file in sourceFiles)
            {
                var content = File.ReadAllText(file);
                MetaPage metaPage = null;
                try
                {
                    Console.WriteLine("Reading file {0}", file);
                    var page = pageParser.Parse(content);

                    metaPage = new MetaPage(file) {Page = page};
                } 
                catch (InvalidPageException ipe)
                {
                    Debug.WriteLine("Invalid YAML Front Matter for file " + file);
                }

                if (metaPage != null)
                    yield return metaPage;
            }
        }
    }
}
