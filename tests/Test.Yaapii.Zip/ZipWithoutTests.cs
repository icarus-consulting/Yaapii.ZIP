using System;
using System.IO;
using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipWithoutTests
    {
        [Fact]
        public void RemovesFile()
        {
            Assert.Empty(
                new ZipPaths(
                    new ZipWithout(
                        "Concerned Citizens",
                        new Zipped("Concerned Citizens", new DeadInput())
                    )
                )
            );
        }

        [Fact]
        public void RejectsUnknownFile()
        {
            Assert.Throws<ArgumentException>(() =>
                new ZipPaths(
                    new ZipWithout(
                        "Concerned Citizens",
                        new Zipped("Brave Citizens", new DeadInput())
                    )
                ).GetEnumerator()
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
        public void RemovesFileFromDifferentZips(string path)
        {
            var zip = new MemoryStream();
            new ResourceOf(path, this.GetType()).Stream().CopyTo(zip);
            zip.Seek(0, SeekOrigin.Begin);

            Assert.DoesNotContain<string>(
                "c/Y/test-a-y-2.txt",
                new ZipPaths(
                    new ZipWithout(
                        "c/Y/test-a-y-2.txt",
                        new InputOf(zip)
                    )
                )
            );
        }
    }
}
