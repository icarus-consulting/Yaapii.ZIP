using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipContainsTests
    {
        [Fact]
        public void ContainsFile()
        {
            Assert.True(
                new ZipContains(
                    new Zipped(
                        "Oh My Dir/My File", new InputOf("Mista Singing Club")
                    ),
                    false,
                    "Oh My Dir/My File"
                ).Value()
            );
        }

        [Fact]
        public void MissesFile()
        {
            Assert.False(
                new ZipContains(
                    new Zipped(
                        "Oh My Dir/My File", new InputOf("Mista Singing Club")
                    ),
                    false,
                    "Oh My Dir/MissedFile"
                ).Value()
            );
        }

        [Fact]
        public void ContainsDirectory()
        {
            Assert.True(
                new ZipContains(
                    new ResourceOf("Datum/example.zip", this.GetType()),
                    false,
                    "raw/KRC/R1"
                ).Value()
            );
        }

        [Fact]
        public void ContainsFiles()
        {
            Assert.True(
                new ZipContains(
                    new ResourceOf("Datum/example.zip", this.GetType()),
                    false,
                    @"raw\KRC\R1\Program\F30\f30_fvr.dat",
                    @"raw\KRC\R1\Program\F30\f30_fvr.src"
                ).Value()
            );
        }

        [Fact]
        public void LeavesOpenAtBegin()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));
            new ZipContains(zip, "Leave me open").Value();

            Assert.Equal(0, zip.Stream().Position);
        }

    }
}
