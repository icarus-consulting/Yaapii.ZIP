using System;
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
