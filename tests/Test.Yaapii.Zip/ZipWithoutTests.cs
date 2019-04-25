using System;
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
    }
}
