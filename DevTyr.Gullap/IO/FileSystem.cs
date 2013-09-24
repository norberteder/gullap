using System.IO;

namespace DevTyr.Gullap.IO
{
    public class FileSystem
    {
        public static void EnsureDirectory(string directory)
        {
            var directoryToEnsure = Path.GetDirectoryName(directory);

            if (!Directory.Exists(directoryToEnsure))
                Directory.CreateDirectory(directoryToEnsure);
        }
    }
}
