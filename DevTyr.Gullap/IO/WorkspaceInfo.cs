using System.Collections.Generic;
using System.IO;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceInfo
    {
        public string WorkspacePath { get; private set; }

        public WorkspaceInfo(string directory)
        {
            WorkspacePath = directory;
        }

        public Dictionary<string, string> GetPathContentMapping()
        {
            if (!Directory.Exists(WorkspacePath))
                throw new DirectoryNotFoundException();

            var sourceFiles = Directory.GetFiles(WorkspacePath, "*.*", SearchOption.AllDirectories);

            var mapping = new Dictionary<string, string>();

            foreach (var file in sourceFiles)
            {
                var content = File.ReadAllText(file);
                mapping.Add(file, content);
            }

            return mapping;
        }
    }
}
