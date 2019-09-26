using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipPathsTests
    {
        [Fact]
        public void ListsFile()
        {
            Assert.Contains(
                "Oh My Dir/My File",
                new ZipPaths(
                    new Zipped(
                        "Oh My Dir/My File", new InputOf("Mista Singing Club")
                    )
                )
            );
        }

        [Fact]
        public void StreamIsAtStart()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));
            new ZipPaths(zip).GetEnumerator();

            Assert.Equal(0, zip.Stream().Position);
        }

        [Fact]
        public void LeavesOpen()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));
            new ZipPaths(zip).GetEnumerator();

            Assert.True(zip.Stream().CanRead);
        }

        [Fact]
        public void IsThreadSafe()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));

            Parallel.For(0, System.Environment.ProcessorCount << 4, (current) =>
            {
                Assert.True(new ZipPaths(zip).GetEnumerator().MoveNext());
            });
        }

        [Fact]
        public void ListsAllPaths()
        {
            var paths =
                new ZipPaths(
                    new ResourceOf("Datum/example.zip", this.GetType())
                );
            Assert.Equal(
                6,
                new Atoms.Enumerable.LengthOf(paths).Value()
            );
        }

        [Theory]
        [InlineData("Datum/7zip.zip")]
        [InlineData("Datum/winrar.zip")]
        [InlineData("Datum/7zip_crypt.zip")]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void ListsAllPathsFromDifferentZips(string path)
        {
            Assert.Equal(
                26,
                new Atoms.Enumerable.LengthOf(
                    new ZipPaths(
                        new ResourceOf(path, this.GetType())
                    )
                ).Value()
            );
        }

        /// <summary>
        /// Windows has only one entry for directories e.g. only
        /// A/X/ and not A/ and A/X/
        /// </summary>
        [Fact]
        public void ListsAllPathsFromWindowsZip()
        {
            Assert.Equal(
                22,
                new Atoms.Enumerable.LengthOf(
                    new ZipPaths(
                        new ResourceOf("Datum/windows.zip", this.GetType())
                    )
                ).Value()
            );
        }
    }
}
