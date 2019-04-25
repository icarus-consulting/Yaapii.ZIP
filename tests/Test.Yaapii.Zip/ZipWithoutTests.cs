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
                        new Zipped("Concerned Citizens", new DeadInput()),
                        "Concerned Citizens"
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
                        new Zipped("Brave Citizens", new DeadInput()),
                        "Concerned Citizens"
                    )
                ).GetEnumerator()
            );
        }
    }
}
