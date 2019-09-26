using System.Collections.Generic;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    /// <summary>
    /// Contents zipped.
    /// </summary>
    public sealed class ZippedTests
    {
        [Fact]
        public void HasContent()
        {
            Assert.Equal(
                "I feel so compressed",
                new TextOf(
                    new ZipExtracted(
                        new Zipped(
                            "small.dat", new InputOf("I feel so compressed")
                        ),
                        "small.dat"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void HasFiles()
        {
            Assert.Equal(
                2,
                new Atoms.Enumerable.LengthOf(
                    new ZipPaths(
                        new Zipped(
                            new Dictionary<string, IInput>()
                            {
                                {"small.dat", new InputOf("I feel so compressed") },
                                {"smaller.dat", new InputOf("I'm so compressed") }
                            }
                        )
                    )
                ).Value()
            );
        }
    }
}
