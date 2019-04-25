using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipContentsTests
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
    }
}
