using System.IO;
using System.Linq;

namespace VAS_API.Extensions
{
    public static class SolutionProvider
    {
        public static DirectoryInfo GetSolutionDirectoryPath()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }
    }
}
