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
                        "aPassword",
                        "filename.txt",
                        new InputOf("a input")
                    ),
                    "filename.txt"
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

        [Fact]
        public void ThrowsOnNoZip()
        {
            Assert.Throws<ArgumentException>(() =>
                new HasPassword(new InputOf("test"), "a path").Value()
            );
        }
    }
}
