using System;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public class ZipWithPasswordTests
    {
        [Fact]
        public void HasFileName()
        {
            Assert.Equal(
                "input.txt", 
                new FirstOf<string>(
                    new Yaapii.Zip.ZipFiles(
                        new ZipWithPassword(
                            "input.txt",
                            "pass",
                            new InputOf("mechiko")
                        )
                    )
                ).Value()
            );
        }

        [Fact]
        public void HasPassword()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new TextOf(
                    new ZipExtracted(
                        new ZipWithPassword("input.txt", "pass", new InputOf("mechiko")),
                        "input.txt"
                    )
                ).AsString();
            });
        }
    }
}
