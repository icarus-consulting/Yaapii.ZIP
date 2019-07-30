[![EO principles respected here](https://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)

# Yaapii.ZIP
Object oriented tools for handling ZIP archives. Based on [Yaapii.Atoms](https://github.com/icarus-consulting/Yaapii.Atoms).

It follows all the rules suggested in the two "[Elegant Objects](https://www.amazon.de/Elegant-Objects-Yegor-Bugayenko/dp/1519166915)" books.

For password protected ZIPs it uses "[DotNetZip](https://github.com/DinoChiesa/DotNetZip)".

## Usage

There are different Decorator classes which accept a input. A input might come from a file or a memorystream or any other stream. You can use Atoms ```InputOf``` to get an input which you can use with this library.

## Only for Zip Archives
This library works with common zip archives, which have a table of contents. 
It does not work with simple compressed streams of data.

### Zip contents

You can create a new ZIP archive like this:

```csharp
var zipArchive =
    new ZipExtracted(
        new Zipped(
            "small.dat", new InputOf("I feel so compressed")
        ),
        "small.dat"
    ).Stream();
```

Update a file in the ZIP:

```csharp
var updated =
    new ZipUpdated(
        new Zipped("Brave Citizens.txt", new InputOf("nobody")),
        "Brave Citizens.txt", new InputOf("Edward Snowden")
    ).Stream();

```

Extract a file in the zip:

```csharp
var extracted =
    new ZipExtracted(
        new InputOf(File.OpenRead("c:/some-zip-archive.zip")),
        "root/dir/some-file.txt",
        false //do not leave the archive open
    ).Stream();
```

List the contents of an archive:

```csharp
IEnumerable<string> contents =
    new ZipPaths(
        new InputOf(File.OpenRead("c:/some-zip-archive.zip")),
    );
```

Remove a file from the archive:

```csharp
var updated =
    new ZipWithout(
        "root/some-file.txt",
        new InputOf(File.OpenRead("c:/some-zip-archive.zip"))
    ).Stream();
```

Change the paths in an archive:

```csharp
var updated =
    new ZipMapped(
        path => "a-prefix" + path,
        new InputOf(File.OpenRead("c:/some-zip-archive.zip"))
    ).Stream();
```

Test if a stream is a valid zip archive:

```csharp
var isZip =
    new IsZipArchive(
        new InputOf(File.OpenRead("c:/some-zip-archive.zip"))
    ).Value();
```

Test if a file in a zip is protected with a password:

```csharp
var hasPassword =
    new HasPassword(
        new InputOf(File.OpenRead("c:/some-zip-archive.zip")),
        "someFileInTheZip.txt")
    ).Value();
```

Extract a file with a password in the zip:

```csharp
var extracted =
    new ZipPasswordExtracted(
        new InputOf(File.OpenRead("c:/some-zip-archive.zip")),
        "root/dir/some-file.txt",
        "password"
    ).Stream();
```

Create a file with a password in a zip:

```csharp
var zipArchiveWithPassword =
    new ZipWithPassword(
        "small.dat",
        new InputOf("I feel so compressed"),
        "password"
    );
```