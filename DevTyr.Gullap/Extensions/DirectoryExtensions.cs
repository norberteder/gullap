using System.IO;
using DevTyr.Gullap.IO;

namespace DevTyr.Gullap.Extensions
{
	public class DirectoryExtensions
	{
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			var dir = new DirectoryInfo(sourceDirName);
			var dirs = dir.GetDirectories();
			
			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			
            FileSystem.EnsureDirectory(destDirName);

            var files = dir.GetFiles();
			foreach (var file in files)
			{
				var temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

		    if (!copySubDirs) return;

		    foreach (var subdir in dirs)
		    {
		        var temppath = Path.Combine(destDirName, subdir.Name);
		        DirectoryCopy(subdir.FullName, temppath, true);
		    }
		}
	}
}

