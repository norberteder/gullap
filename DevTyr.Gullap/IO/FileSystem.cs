using System.IO;

namespace DevTyr.Gullap.IO
{
    public class FileSystem
    {
        public static void EnsureDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
