using FluentAssertions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Labuladuo.PathExtensions.Tests;

public class FileSystemInfoExtensionsTests {
    [Fact]
    public void GIVEN_DirectoryInfo_and_fileName_WHEN_GetFileInfo_THEN_returns_valid_FileInfo() {
        DirectoryInfo directoryInfo = new("./folder");
        const string fileName = "file_name";

        FileInfo fileInfo = directoryInfo.GetFileInfo(fileName);

        fileInfo.Name.Should().Be(fileName);
        fileInfo.Directory!.Name.Should().Be("folder");
    }

    [Fact]
    public void GIVEN_DirectoryInfo_and_null_fileName_WHEN_GetFileInfo_THEN_throws_exception() {
        DirectoryInfo directoryInfo = new("./folder");

        Assert.Throws<ArgumentException>(() => directoryInfo.GetFileInfo(null!));
    }

    [Fact]
    public void GIVEN_null_DirectoryInfo_and_fileName_WHEN_GetFileInfo_THEN_throws_exception() {
        DirectoryInfo? directoryInfo = null;

        Assert.Throws<NullReferenceException>(() => directoryInfo!.GetFileInfo("file_name"));
    }

    [Fact]
    public void GIVEN_DirectoryInfo_and_empty_fileName_WHEN_GetFileInfo_THEN_throws_exception() {
        DirectoryInfo directoryInfo = new("./folder");

        Assert.Throws<ArgumentException>(() => directoryInfo.GetFileInfo(""));
    }

    [Fact]
    public void GIVEN_existing_DirectoryInfo_and_name_of_existing_file_WHEN_CreateFile_THEN_overrides_file() {
        string filePath = Path.GetTempFileName();
        string fileName = Path.GetFileName(filePath);
        WriteRandomDataToFile(filePath);

        FileInfo fileInfo = CreateExistingDirectoryInfo().CreateFile(fileName);

        fileInfo.Exists.Should().BeTrue();
        fileInfo.Length.Should().Be(0);
    }

    [Fact]
    public void GIVEN_existing_DirectoryInfo_and_name_of_non_existing_file_WHEN_CreateFile_THEN_creates_new_file() {
        string fileName = Path.GetRandomFileName();

        FileInfo fileInfo = CreateExistingDirectoryInfo().CreateFile(fileName);

        fileInfo.Exists.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_non_existing_DirectoryInfo_and_fileName_WHEN_CreateFile_THEN_creates_directory_recursively_and_creates_new_file() {
        string fileName = Path.GetRandomFileName();

        FileInfo fileInfo = CreateNonExistingDirectoryInfo().CreateFile(fileName);

        fileInfo.Exists.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_non_existing_file_WHEN_CreateFileWithContent_THEN_file_has_the_content() {
        FileInfo fileInfo = CreateNonExistingFile();

        string content = GetRandomText();

        fileInfo.CreateFileWithContent(ToStream(content));

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public void GIVEN_existing_file_WHEN_CreateFileWithContent_THEN_file_is_overriden() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        string content = GetRandomText();

        fileInfo.CreateFileWithContent(ToStream(content));

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public void GIVEN_file_WHEN_CreateFileWithContent_THEN_input_stream_is_not_disposed() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        Stream stream = ToStream(GetRandomText());

        fileInfo.CreateFileWithContent(stream);

        stream.CanRead.Should().BeTrue();
    }

    [Fact]
    public async Task GIVEN_non_existing_file_WHEN_CreateFileWithContentAsync_THEN_file_has_the_content() {
        FileInfo fileInfo = CreateNonExistingFile();

        string content = GetRandomText();

        await fileInfo.CreateFileWithContentAsync(ToStream(content));

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_existing_file_WHEN_CreateFileWithContentAsync_THEN_file_is_overriden() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        string content = GetRandomText();

        await fileInfo.CreateFileWithContentAsync(ToStream(content));

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_file_WHEN_CreateFileWithContentAsync_THEN_input_stream_is_not_disposed() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        Stream stream = ToStream(GetRandomText());

        await fileInfo.CreateFileWithContentAsync(stream);

        stream.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_non_existing_file_WHEN_CreateFileWithText_THEN_file_has_the_content() {
        FileInfo fileInfo = CreateNonExistingFile();

        string content = GetRandomText();

        fileInfo.CreateFileWithText(content);

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public void GIVEN_existing_file_WHEN_CreateFileWithText_THEN_file_is_overriden() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        string content = GetRandomText();

        fileInfo.CreateFileWithText(content);

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_non_existing_file_WHEN_CreateFileWithTextAsync_THEN_file_has_the_content() {
        FileInfo fileInfo = CreateNonExistingFile();

        string content = GetRandomText();

        await fileInfo.CreateFileWithTextAsync(content);

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_existing_file_WHEN_CreateFileWithTextAsync_THEN_file_is_overriden() {
        FileInfo fileInfo = CreateExistingFileWithContent();

        string content = GetRandomText();

        await fileInfo.CreateFileWithTextAsync(content);

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public void GIVEN_non_existing_directory_and_fileName_WHEN_CreateFileWithText_THEN_file_has_the_content() {
        DirectoryInfo directoryInfo = CreateNonExistingDirectoryInfo();
        string fileName = Path.GetRandomFileName();

        string content = GetRandomText();

        FileInfo fileInfo = directoryInfo.CreateFileWithText(fileName, content);

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_non_existing_directory_and_fileName_WHEN_CreateFileWithTextAsync_THEN_file_has_the_content() {
        DirectoryInfo directoryInfo = CreateNonExistingDirectoryInfo();
        string fileName = Path.GetRandomFileName();

        string content = GetRandomText();

        FileInfo fileInfo = await directoryInfo.CreateFileWithTextAsync(fileName, content);

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public void GIVEN_non_existing_directory_and_fileName_WHEN_CreateFileWithContent_THEN_file_has_the_content() {
        DirectoryInfo directoryInfo = CreateNonExistingDirectoryInfo();
        string fileName = Path.GetRandomFileName();

        string content = GetRandomText();

        FileInfo fileInfo = directoryInfo.CreateFileWithContent(fileName, ToStream(content));

        fileInfo.OpenText().ReadToEnd().Should().Be(content);
    }

    [Fact]
    public async Task GIVEN_non_existing_directory_and_fileName_WHEN_CreateFileWithContentAsync_THEN_file_has_the_content() {
        DirectoryInfo directoryInfo = CreateNonExistingDirectoryInfo();
        string fileName = Path.GetRandomFileName();

        string content = GetRandomText();

        FileInfo fileInfo = await directoryInfo.CreateFileWithContentAsync(fileName, ToStream(content));

        (await fileInfo.OpenText().ReadToEndAsync()).Should().Be(content);
    }

    [Fact]
    public void GIVEN_non_existing_directory_and_subDirectoryName_WHEN_CreateSubDirectory_THEN_the_directory_exists() {
        DirectoryInfo directoryInfo = CreateNonExistingDirectoryInfo();

        DirectoryInfo subDirectoryInfo = directoryInfo.CreateSubdirectory(Path.GetRandomFileName());

        subDirectoryInfo.Exists.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_existing_directory_and_non_existing_subDirectoryName_WHEN_CreateSubDirectory_THEN_the_directory_exists() {
        DirectoryInfo directoryInfo = CreateExistingDirectoryInfo();

        DirectoryInfo subDirectoryInfo = directoryInfo.CreateSubdirectory(Path.GetRandomFileName());

        subDirectoryInfo.Exists.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_existing_directory_and_existing_subDirectoryName_WHEN_CreateSubDirectory_THEN_does_not_override() {
        DirectoryInfo directoryInfo = CreateExistingDirectoryInfo();
        DirectoryInfo subDirectoryInfo = directoryInfo.CreateSubdirectory(Path.GetRandomFileName());
        DirectoryInfo subSubDirectoryInfo = subDirectoryInfo.CreateSubdirectory(Path.GetRandomFileName());

        directoryInfo.CreateSubdirectory(subDirectoryInfo.Name);

        subSubDirectoryInfo.Exists.Should().BeTrue();
    }

    [Fact]
    public void GIVEN_directory1_and_directory2_WHEN_get_RelativePath_THEN_gives_path_from_directory1_to_directory2() {
        DirectoryInfo from = new("/a/b/c");
        DirectoryInfo to = new("/a/b/d");
        from.RelativePath(to).Should().Be(Path.Combine("..", "d") + Path.DirectorySeparatorChar);
    }

    [Fact]
    public void GIVEN_directory_and_file_WHEN_get_RelativePath_THEN_gives_path_from_directory_to_file() {
        DirectoryInfo from = new("/a/b/c");
        FileInfo to = new("/a/b/d.txt");
        from.RelativePath(to).Should().Be(Path.Combine("..", "d.txt"));
    }

    [Fact]
    public void GIVEN_file_and_directory_WHEN_get_RelativePath_THEN_gives_path_from_file_to_directory() {
        FileInfo from = new("/a/b/c.txt");
        DirectoryInfo to = new("/a/b/d");
        from.RelativePath(to).Should().Be("d" + Path.DirectorySeparatorChar);
    }

    [Fact]
    public void GIVEN_file1_and_file2_WHEN_get_RelativePath_THEN_gives_path_from_file1_to_file2() {
        FileInfo from = new("/a/b/f/c.txt");
        FileInfo to = new("/a/b/d/e.txt");
        from.RelativePath(to).Should().Be(Path.Combine("..", "d", "e.txt"));
    }

    [Fact]
    public void GIVEN_file1_and_file2_in_the_same_directory_WHEN_get_RelativePath_THEN_returns_path_to_file2() {
        FileInfo from = new("/a/b/c.txt");
        FileInfo to = new("/a/b/e.txt");
        from.RelativePath(to).Should().Be("." + Path.DirectorySeparatorChar + "e.txt");
    }

    [Fact]
    public void GIVEN_directory1_and_directory2_are_same_WHEN_get_RelativePath_THEN_returns_dot() {
        DirectoryInfo from = new("/a/b/c");
        DirectoryInfo to = new("/a/b/c");
        from.RelativePath(to).Should().Be("." + Path.DirectorySeparatorChar);
    }

    static DirectoryInfo CreateNonExistingDirectoryInfo() =>
        new(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), Path.GetRandomFileName()));

    static DirectoryInfo CreateExistingDirectoryInfo() =>
        new(Path.GetTempPath());

    static FileInfo CreateNonExistingFile() =>
        new(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

    static FileInfo CreateExistingFileWithContent() {
        string filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        WriteRandomDataToFile(filePath);
        return new(filePath);
    }

    static string GetRandomText() =>
        Guid.NewGuid().ToString();

    static Stream ToStream(string content) =>
        new MemoryStream(Encoding.UTF8.GetBytes(content));

    static void WriteRandomDataToFile(string filePath) =>
        File.WriteAllText(filePath, GetRandomText());
}