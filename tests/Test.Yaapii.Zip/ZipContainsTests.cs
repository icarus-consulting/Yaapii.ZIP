using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public sealed class ZipContainsTests
    {
        [Fact]
        public void FindsContentInZippedFile()
        {           
           Assert.True(
                    new ZipContains(
                         new ResourceOf("Datum/example.zip", this.GetType()),
                    "raw/KRC/R1/Program/F30/f30_fvr.dat"
                ).Value()
            ); 
        }

        [Fact]
        public void FindsContentInSelfCreated()
        {
            Assert.True(
                new ZipContains(
                   new Zipped(
                        "Oh My Dir/My File", new InputOf("Mista Singing Club")
                    ), "Oh My Dir/My File"
                ).Value()
            );
        }

        [Fact]
        public void FalseIfFileIsNotContained()
        {
            Assert.False(
               new ZipContains(
                   new Zipped(
                        "Oh My Dir/My File", new InputOf("Mista Singing Club")
                    ), "Oh My Dir/Not My File"
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
        public void FindsContentInDifferentZips(string path)
        {
            Assert.True(
                  new ZipContains(
                       new ResourceOf(path, this.GetType()),
                  "A/X/test-a-x-1.txt"
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
        public void FalseInDifferentZips(string path)
        {
            Assert.False(
                  new ZipContains(
                       new ResourceOf(path, this.GetType()),
                       "this/is/not/the/file/you/are/looking.for"
              ).Value()
          );
        }

        [Fact]
        public void IsThreadSafe()
        {
            var zip = new Zipped("Leave me open", new InputOf("Please!"));

            Parallel.For(0, System.Environment.ProcessorCount << 4, (current) =>
            {
                Assert.True(
                    new ZipContains(
                            zip,
                            "Leave me open"
                        ).Value()
                    );
            });
        }
    }
}
