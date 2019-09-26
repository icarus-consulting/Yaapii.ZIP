using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipFilesTests
    {
        [Fact]
        public void ListsFile()
        {
            Assert.Contains(
                "Oh My Dir/My File",
                new ZipFiles(
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
            new ZipFiles(zip).GetEnumerator();

            Assert.Equal(0, zip.Stream().Position);
        }

        [Fact]
        public void LeavesOpen()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));
            new ZipFiles(zip).GetEnumerator();

            Assert.True(zip.Stream().CanRead);
        }

        [Fact]
        public void IsThreadSafe()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));

            Parallel.For(0, System.Environment.ProcessorCount << 4, (current) =>
            {
                Assert.True(new ZipFiles(zip).GetEnumerator().MoveNext());
            });
        }

        [Fact]
        public void ListsOnlyFiles()
        {
            Assert.Equal(
                2,
                new Atoms.Enumerable.LengthOf(
                    new ZipFiles(
                        new ResourceOf("Datum/example.zip", this.GetType())
                    )
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
        public void ListsOnlyFilesFromDifferentZips(string path)
        {
            Assert.Equal(
                16,
                new Atoms.Enumerable.LengthOf(
                    new ZipFiles(
                        new ResourceOf(path, this.GetType())
                    )
                ).Value()
            );
        }
    }
}
