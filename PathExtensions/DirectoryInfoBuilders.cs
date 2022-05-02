using System.IO;

namespace Labuladuo.PathExtensions {
    public static class DirectoryInfoBuilders {
        /// <summary>
        /// Creates a new temporary directory inside system temp directory
        /// </summary>
        /// <returns>Newly created directory with a random name</returns>
        public static DirectoryInfo CreateTempDirectory() =>
            GetTempDirectory().CreateSubDirectory(Path.GetRandomFileName());

        /// <summary>
        /// Same as Path.GetTempPath(), but returns strongly typed result
        /// </summary>
        /// <returns>System temp directory
        /// (directory containing temporary files and directories)
        /// as DirectoryInfo</returns>
        public static DirectoryInfo GetTempDirectory() =>
            new DirectoryInfo(Path.GetTempPath());

        /// <summary>
        /// Same as Directory.GetCurrentDirectory(), but returns strongly typed result
        /// </summary>
        /// <returns>Current directory as DirectoryInfo</returns>
        public static DirectoryInfo GetCurrentDirectory() =>
            new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}