using Xunit;
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
    }
}
