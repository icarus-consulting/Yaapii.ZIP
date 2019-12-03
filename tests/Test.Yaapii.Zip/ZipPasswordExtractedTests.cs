using System;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public class ZipPasswordExtractedTests
    {
        [Fact]
        public void FailsOnWrongPassword()
        {
            Assert.Throws<ArgumentException>(() =>
                new ZipPasswordExtracted(
                    new ZipWithPassword(
                        "text.txt",
                        "password",
                        new InputOf("safe")
                    ),
                    "text.txt",
                    "noPassword"
                ).Stream()
            );
        }

        [Fact]
        public void ReturnsExtractedInput()
        {
            Assert.Equal(
                "safe",
                new TextOf(
                    new ZipPasswordExtracted(
                        new ZipWithPassword(
                            "text.txt",
                            "password",
                            new InputOf("safe")
                        ),
                        "text.txt",
                        "password"
                    )
                ).AsString()
            );
        }

        [Theory]
        [InlineData("Datum/7zip_crypt.zip")]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void ExtractsFromDifferentZips(string path)
        {
            Assert.Equal(
                "123",
                new TextOf(
                    new ZipPasswordExtracted(
                        new ResourceOf(path, this.GetType()),
                         @"c\Y\test-a-y-1.txt",
                        "icarus"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void ExtractsUnencryptedZip()
        {
            Assert.Equal(
                "unsafe",
                new TextOf(
                    new ZipPasswordExtracted(
                        new Zipped(
                            "text.txt",
                            new InputOf("unsafe")
                        ),
                        "text.txt",
                        "password"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void FailsOnMissingEntry()
        {
            Assert.Throws<ArgumentException>(() =>
                new ZipPasswordExtracted(
                    new ZipWithPassword(
                        "filename.txt",
                        "pwd",
                        new InputOf("a input")
                    ),
                    "otherfile.txt",
                    "pwd"
                ).Stream()
            );
        }
    }
}
