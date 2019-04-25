using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class IsZipArchiveTests
    {
        [Fact]
        public void DetectsZip()
        {
            Assert.True(
                new IsZipArchive(
                    new Zipped("some/path/file.txt", new InputOf("Some Data"))
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
