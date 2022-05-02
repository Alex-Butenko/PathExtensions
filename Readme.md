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
