using System.IO.Compression;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipMappedTests
    {
        [Fact]
        public void Maps()
        {
            var mapped =
                new ZipMapped(
                    path => "directory/content",
                    new Zipped("original path/content", new InputOf("Map me if you can"))
                );

            using (var zip = new ZipArchive(mapped.Stream(), ZipArchiveMode.Read, false))
            {
                Assert.Contains(
                    "directory/content",
                    new ItemAt<ZipArchiveEntry>(
                        zip.Entries
                    ).Value().FullName
                );
            }
        }

        [Theory]
        [InlineData("Datum/windows.zip")]
        [InlineData("Datum/7zip.zip")]
        [InlineData("Datum/winrar.zip")]
        [InlineData("Datum/7zip_crypt.zip")] //for whatever reason that works!?
        public void MapsDifferentZips(string path)
        {
            Assert.StartsWith(
               "root/",
                new ItemAt<string>(
                    new ZipPaths(
                        new ZipMapped(
                            entry => "root/" + entry,
                            new ResourceOf(path, this.GetType())
                        )
                    )
                ).Value()
            );
        }

        [Theory]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void ThrowsCryptedZips(string path)
        {
            Assert.Throws<System.IO.InvalidDataException>(() =>
                new ZipMapped(
                    entry => "root/" + entry,
                    new ResourceOf(path, this.GetType())
                ).Stream()
            );
        }
    }
}
