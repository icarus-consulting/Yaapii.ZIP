using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class IsZipArchiveTests
    {
        [Fact]
        public void DetectsOwnZip()
        {
            Assert.True(
                new IsZipArchive(
                    new Zipped("some/path/file.txt", new InputOf("Some Data"))
                ).Value()
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
        public void DetectsDifferentZips(string path)
        {
            Assert.True(
               new IsZipArchive(
                   new ResourceOf(path, this.GetType())
               ).Value()
           );
        }

        [Fact]
        public void DetectsNonZip()
        {
            Assert.False(
                new IsZipArchive(
                    new InputOf("not a zip")
                ).Value()
            );
        }

        [Fact]
        public void DetectsEmptyZip()
        {
            Assert.True(
                new IsZipArchive(
                    new ZipWithout("some/path/file.txt",
                        new Zipped("some/path/file.txt", new InputOf("Some Data"))
                    )
                ).Value()
            );
        }
    }
}
