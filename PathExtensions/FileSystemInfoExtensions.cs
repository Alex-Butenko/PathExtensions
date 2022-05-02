using System;
using System.IO;
using System.Threading.Tasks;

namespace Labuladuo.PathExtensions {
    public static class FileSystemInfoExtensions {
        /// <summary>
        /// Creates a file. If file already exists, overrides it.
        /// </summary>
        /// <param name="directoryInfo">Directory where to create the file.
        /// If directory does not exist, creates it recursively together with all parent directories, if necessary</param>
        /// <param name="fileName">Name of the file to create</param>
        /// <returns>Created file</returns>
        public static FileInfo CreateFile(this DirectoryInfo directoryInfo, string fileName) {
            FileInfo file = directoryInfo.GetFileInfo(fileName);
            directoryInfo.Create();
            file.Create();
            return file;
        }

        /// <summary>
        /// Creates a file with predefined content. If file already exists, overrides it.
        /// If directory does not exist, creates it, recursively.
        /// </summary>
        /// <param name="directoryInfo">Directory where the file is located</param>
        /// <param name="fileName">Name of the file to create</param>
        /// <param name="stream">Stream to read content from. Closing the stream is the duty of the calling code</param>
        /// <returns>FileInfo of created file</returns>
        public static FileInfo CreateFileWithContent(this DirectoryInfo directoryInfo, string fileName, Stream stream) =>
            directoryInfo.GetFileInfo(fileName).CreateFileWithContent(stream);

        /// <summary>
        /// Creates a file with predefined content, asynchronously. If file already exists, overrides it.
        /// If directory does not exist, creates it, recursively.
        /// </summary>
        /// <param name="directoryInfo">Directory where the file is located</param>
        /// <param name="fileName">Name of the file to create</param>
        /// <param name="stream">Stream to read content from. Closing the stream is the duty of the calling code</param>
        /// <returns>FileInfo of created file</returns>
        public static Task<FileInfo> CreateFileWithContentAsync(this DirectoryInfo directoryInfo, string fileName, Stream stream) =>
            directoryInfo.GetFileInfo(fileName).CreateFileWithContentAsync(stream);

        /// <summary>
        /// Creates a file with predefined text. If file already exists, overrides it.
        /// If directory does not exist, creates it, recursively.
        /// </summary>
        /// <param name="directoryInfo">Directory where the file is located</param>
        /// <param name="fileName">Name of the file to create</param>
        /// <param name="text">Text for the file</param>
        /// <returns>FileInfo of created file</returns>
        public static FileInfo CreateFileWithText(this DirectoryInfo directoryInfo, string fileName, string text) =>
            directoryInfo.GetFileInfo(fileName).CreateFileWithText(text);

        /// <summary>
        /// Creates a file with predefined text, asynchronously. If file already exists, overrides it.
        /// If directory does not exist, creates it, recursively.
        /// </summary>
        /// <param name="directoryInfo">Directory where the file is located</param>
        /// <param name="fileName">Name of the file to create</param>
        /// <param name="text">Text for the file</param>
        /// <returns>FileInfo of created file</returns>
        public static Task<FileInfo> CreateFileWithTextAsync(this DirectoryInfo directoryInfo, string fileName, string text) =>
            directoryInfo.GetFileInfo(fileName).CreateFileWithTextAsync(text);

        /// <summary>
        /// Creates a file with predefined content. If file already exists, overrides it.
        /// </summary>
        /// <param name="fileInfo">File to create</param>
        /// <param name="stream">Stream to read content from. Closing the stream is the duty of the calling code</param>
        /// <returns>Same as input <paramref name="fileInfo"/></returns>
        public static FileInfo CreateFileWithContent(this FileInfo fileInfo, Stream stream) {
            fileInfo.Directory?.Create();

            using FileStream fs = fileInfo.Create();

            stream.CopyTo(fs);
            fs.Flush();
            return fileInfo;
        }

        /// <summary>
        /// Creates a file with predefined content, asynchronously. If file already exists, overrides it.
        /// </summary>
        /// <param name="fileInfo">File to create</param>
        /// <param name="stream">Stream to read content from. Closing the stream is the duty of the calling code</param>
        /// <returns>Same as input <paramref name="fileInfo"/></returns>
        public static async Task<FileInfo> CreateFileWithContentAsync(this FileInfo fileInfo, Stream stream) {
            fileInfo.Directory?.Create();

            await using FileStream fs = fileInfo.Create();

            await stream.CopyToAsync(fs);
            await fs.FlushAsync();
            return fileInfo;
        }

        /// <summary>
        /// Creates a file with predefined text. If file already exists, overrides it.
        /// </summary>
        /// <param name="fileInfo">File to create</param>
        /// <param name="text">Text for the file</param>
        /// <returns>Same as input <paramref name="fileInfo"/></returns>
        public static FileInfo CreateFileWithText(this FileInfo fileInfo, string text) {
            fileInfo.Directory?.Create();

            using StreamWriter sw = fileInfo.CreateText();

            sw.Write(text);
            sw.Flush();
            return fileInfo;
        }

        /// <summary>
        /// Creates a file with predefined text, asynchronously. If file already exists, overrides it.
        /// </summary>
        /// <param name="fileInfo">File to create</param>
        /// <param name="text">Text for the file</param>
        /// <returns>Same as input <paramref name="fileInfo"/></returns>
        public static async Task<FileInfo> CreateFileWithTextAsync(this FileInfo fileInfo, string text) {
            fileInfo.Directory?.Create();

            await using StreamWriter sw = fileInfo.CreateText();

            await sw.WriteAsync(text);
            await sw.FlushAsync();
            return fileInfo;
        }

        /// <summary>
        /// Creates a directory. If parent directory does not exists, Also creates it, recursively
        /// If directory already exists, this method does not do anything and does not override existing directory.
        /// </summary>
        /// <param name="directoryInfo">Parent directory</param>
        /// <param name="subDirectoryName">Name of created directory</param>
        /// <returns>Newly created directory</returns>
        public static DirectoryInfo CreateSubDirectory(this DirectoryInfo directoryInfo, string subDirectoryName) {
            DirectoryInfo subDir = directoryInfo.Combine(subDirectoryName);
            subDir.Create();
            return subDir;
        }

        /// <summary>
        /// Same as Path.Combine() but for DirectoryInfo
        /// </summary>
        /// <param name="directoryInfo">Parent directory</param>
        /// <param name="directoryName">Sub-directory</param>
        /// <returns>Resulting DirectoryInfo from combination of <paramref name="directoryInfo"/> and <paramref name="directoryName"/>
        /// Note that it does not create the directory</returns>
        public static DirectoryInfo Combine(this DirectoryInfo directoryInfo, string directoryName) =>
            new DirectoryInfo(Path.Combine(directoryInfo.FullName, directoryName));

        /// <summary>
        /// Create a FileInfo. Note that directory or file do not have to exist.
        /// </summary>
        /// <param name="directoryInfo">Parent directory for the file</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>FileInfo from combination of <paramref name="directoryInfo"/> and <paramref name="fileName"/></returns>
        public static FileInfo GetFileInfo(this DirectoryInfo directoryInfo, string fileName) =>
            string.IsNullOrWhiteSpace(fileName)
                ? throw new ArgumentException("Must have a value", nameof(fileName))
                : new FileInfo(Path.Combine(directoryInfo.FullName, fileName));

        /// <summary>
        /// Finds relative path from one file or folder to another. Based and compatible with Path.GetRelativePath()
        /// </summary>
        /// <param name="from">Contains the starting directory. If it is a file, parent directory is the starting one.</param>
        /// <param name="to">Contains the endpoint directory or file.</param>
        /// <returns>The relative path from the start directory to the end path or <paramref name="to"/>
        /// if the paths are not related.</returns>
        public static string RelativePath(this FileSystemInfo from, FileSystemInfo to) {
            static string GetDirPath(FileSystemInfo fsInfo) =>
                fsInfo switch {
                    FileInfo fileInfo => fileInfo.Directory?.FullName
                        ?? throw new ArgumentException("Cannot identify parent directory", nameof(fileInfo)),
                    _ => fsInfo.FullName
                };

            string relativePath = Path.GetRelativePath(GetDirPath(from), GetDirPath(to));

            return to switch {
                FileInfo fileInfo => Path.Combine(relativePath, fileInfo.Name),
                _ => relativePath + Path.DirectorySeparatorChar
            };
        }
    }
}