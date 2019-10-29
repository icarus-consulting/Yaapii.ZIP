using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public sealed class ZipUpdatedTests
    {
        [Fact]
        public void UpdatesFile()
        {
            Assert.Equal(
                "Edward Snowden",
                new TextOf(
                    new ZipExtracted(
                        new ZipUpdated(
                            new Zipped("Brave Citizens.txt", new InputOf("")),
                            "Brave Citizens.txt", new InputOf("Edward Snowden")
                        ),
                        "Brave Citizens.txt"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void ThrowsForPasswordFile()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ZipUpdated(
                    new ZipWithPassword(
                        "pwd",
                        "Brave Citizens.txt",
                        new InputOf("Empty data will not be encrypted!")
                    ),
                    "Brave Citizens.txt",
                    new InputOf("new content")
                ).Stream()
            );
        }

        [Theory]
        [InlineData("Datum/windows.zip")]
        [InlineData("Datum/7zip.zip")]
        [InlineData("Datum/winrar.zip")]
        public void UpdatesFileInDifferentZips(string path)
        {
            var zip = new MemoryStream();
            new ResourceOf(path, this.GetType()).Stream().CopyTo(zip);
            zip.Seek(0, SeekOrigin.Begin);

            Assert.Equal(
                "456",
                new TextOf(
                    new ZipExtracted(
                        new ZipUpdated(
                            new InputOf(zip),
                            "c/Y/test-a-y-2.txt",
                            new InputOf("456")
                        ),
                        "c/Y/test-a-y-2.txt"
                    )
                ).AsString()
            );
        }

        [Theory]
        [InlineData("Datum/7zip_crypt.zip")]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void ThrowsForDifferentCrypedZips(string path)
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ZipUpdated(
                    new ResourceOf(path, this.GetType()),
                    "c/Y/test-a-y-2.txt",
                    new InputOf("456")
                ).Stream()
            );
        }

        [Theory]
        [InlineData("Datum/windows.zip")]
        [InlineData("Datum/7zip.zip")]
        [InlineData("Datum/winrar.zip")]
        [InlineData("Datum/7zip_crypt.zip")]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void AddsFileToDifferentZips(string path)
        {
            var zip = new MemoryStream();
            new ResourceOf(path, this.GetType()).Stream().CopyTo(zip);
            zip.Seek(0, SeekOrigin.Begin);

            Assert.Equal(
                "new text",
                new TextOf(
                    new ZipExtracted(
                        new ZipUpdated(
                            new InputOf(zip),
                            "c/Y/newfile.txt",
                            new InputOf("new text")
                        ),
                        "c/Y/newfile.txt"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void CreatesUnknownFile()
        {
            Assert.Contains(
                "concerned citizens",
                new ZipPaths(
                    new ZipUpdated(
                        new Zipped("Brave Citizens", new DeadInput()),
                        "concerned citizens",
                        new InputOf("Donald")
                    )
                )
            );
        }

        [Fact]
        public void IsThreadSafe()
        {
            var zip = new Zipped("Status", new InputOf(@"I exist \o/"));
            Parallel.For(0, Environment.ProcessorCount << 4, (current) =>
            {

            });
        }
    }
}
