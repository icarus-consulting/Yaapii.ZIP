using System;
using System.Collections.Generic;
using System.Text;
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
            var name = new FirstOf<string>(
                new Yaapii.Zip.ZipFiles(
                    new ZipWithPassword(
                        "input.txt",
                        "pass",
                        new InputOf("mechiko")
                    )
                )
            ).Value();
            Assert.Equal("input.txt", name);
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
