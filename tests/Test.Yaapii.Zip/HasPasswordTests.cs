using System;
using Xunit;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public class HasPasswordTests
    {
        [Fact]
        public void ReturnsTrueOnPassword()
        {
            Assert.True(
                new HasPassword(
                    new ZipWithPassword(
                        "filename.txt",
                        "aPassword",
                        new InputOf("a input")
                    ),
                    "filename.txt"
                ).Value()
            );
        }

        [Theory]
        [InlineData("Datum/7zip_crypt.zip")]
        [InlineData("Datum/7zip_crypt_aes.zip")]
        [InlineData("Datum/winrar_crypt.zip")]
        [InlineData("Datum/winrar_crypt_aes.zip")]
        public void HasPasswordFromDifferentZips(string path)
        {
            Assert.True(
                new HasPassword(
                    new ResourceOf(path, this.GetType()),
                    @"c\Y\test-a-y-1.txt"
                ).Value()
            );
        }

        [Fact]
        public void ReturnsFalseOnNoPassword()
        {
            Assert.False(
                new HasPassword(
                    new Zipped(
                        "filename.txt",
                        new InputOf("a input")
                    ),
                    "filename.txt"
                ).Value()
            );
        }

        [Theory]
        [InlineData("Datum/windows.zip")]
        [InlineData("Datum/7zip.zip")]
        [InlineData("Datum/winrar.zip")]
        public void HasNoPasswordFromDifferentZips(string path)
        {
            Assert.False(
                new HasPassword(
                    new ResourceOf(path, this.GetType()),
                    @"c\Y\test-a-y-1.txt"
                ).Value()
            );
        }

        [Fact]
        public void ThrowsOnNoZip()
        {
            Assert.Throws<ArgumentException>(() =>
                new HasPassword(new InputOf("test"), "a path").Value()
            );
        }
    }
}
