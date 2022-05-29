# PathExtensions
Dotnet has lots of useful apis for work with files and directories.
However, using them it always feels like there are actually a few sets of overlapping
tools.
* One is DirectoryInfo/FileInfo - strongly typed wrappers for directories and files.
However, it lacks most of useful methods to interact with other DirectoryInfo/FileInfo
instances.
* Second is File and Directory - static classes that are good in every aspect except
that they only take strings as arguments for paths - not so much fun for people who got
used to write strongly typed code
* Third is Path, Swiss army knife for different methods related to path. unfortunately,
can only use strings as input.

So, the purpose of this project is to create a few wrappers and extensions so if would be
possible to use same methods as in Path, File and Directory, but also having all
advantages of DirectoryInfo and FileInfo as arguments and return types.

## Some examples:
Create a text file in directory with defined content:

```csharp
FileInfo fileInfo = directoryInfo.CreateFileWithText(fileName, content);
```

Combine paths (same way as Path.Combine):

```csharp
DirectoryInfo subDir = directoryInfo.Combine(subDirectoryName);
```

Get a relative path. Works same way as Path.GetRelativePath, but takes into account if variables are FileInfo or DirectoryInfo:

```csharp
DirectoryInfo from = new("/a/b/c");
DirectoryInfo to = new("/a/b/d");
string relativePath = from.RelativePath(to); // relativePath == "../d"

DirectoryInfo from = new("/a/b/c");
FileInfo to = new("/a/b/d.txt");
string relativePath = from.RelativePath(to); // relativePath == "../d.txt"

FileInfo from = new("/a/b/f/c.txt");
FileInfo to = new("/a/b/d/e.txt");
string relativePath = from.RelativePath(to); // relativePath == "../d/e.txt"
```

Some other useful methods:

```csharp
DirectoryInfo tempDirectory = DirectoryInfoBuilders.CreateTempDirectory();
DirectoryInfo currentDirectory = DirectoryInfoBuilders.GetCurrentDirectory();
```
