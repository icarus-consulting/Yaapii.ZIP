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
                        "password",
                        "text.txt",
                        new InputOf("safe")
                    ),
                    "noPassword",
                    "text.txt"
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
                            "password",
                            "text.txt",
                            new InputOf("safe")
                        ),
                        "password",
                        "text.txt"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void LeavesStreamOpen()
        {
            var encrypted =
                new ZipWithPassword(
                    "password",
                    "text.txt",
                    new InputOf("safe")
                ).Stream();

            new ZipPasswordExtracted(
                new InputOf(encrypted),
                "password",
                "text.txt",
                true
            ).Stream();

            Assert.True(encrypted.CanRead);
        }

        [Fact]
        public void ClosesStream()
        {
            var encrypted =
                new ZipWithPassword(
                    "password",
                    "text.txt",
                    new InputOf("safe")
                ).Stream();

            new ZipPasswordExtracted(
                new InputOf(encrypted),
                "password",
                "text.txt",
                false
            ).Stream();

            Assert.False(encrypted.CanRead);
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
                        "icarus",
                         @"c\Y\test-a-y-1.txt"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void FailsOnNoPassword()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new TextOf(
                    new ZipPasswordExtracted(
                        new Zipped(
                            "text.txt",
                            new InputOf("safe")
                        ),
                        "password",
                        "text.txt"
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
