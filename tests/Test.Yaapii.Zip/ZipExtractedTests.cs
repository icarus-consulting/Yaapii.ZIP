using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public sealed class ZipExtractedTests
    {
        [Fact]
        public void ExtractsContent()
        {
            Assert.Equal(
                "Rock you like a hurricane",
                new TextOf(
                    new ZipExtracted(
                        new Zipped("here I am", new InputOf("Rock you like a hurricane")),
                        "here I am",
                        false
                    )
                )
                .AsString()
            );
        }

        [Theory]
        [InlineData("KRC/R1/Program/F20/f20_rur.src")]
        [InlineData(@"KRC\R1\Program\F20\f20_rur.src")]
        public void WorksWithSlashAndBacklashSeparation(string path)
        {
            Assert.NotEmpty(
                new TextOf(
                    new ZipExtracted(
                        new Zipped(path, new InputOf("Moin")),
                        path,
                        false
                    )
                )
                .AsString()
            );
        }
    }
}
