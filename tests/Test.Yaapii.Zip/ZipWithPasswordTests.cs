using System;
using System.Collections.Generic;
using Xunit;
using Yaapii.Atoms;
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
                        "pass",
                        "input.txt",
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
                        new ZipWithPassword(
                            "pass",
                            "input.txt",
                            new InputOf("mechiko")
                        ),
                        "input.txt"
                    )
                ).AsString();
            });
        }

        [Fact]
        public void ZipsMultipleFiles()
        {
            Assert.Equal(
                2,
                new Atoms.Enumerable.LengthOf(
                    new ZipFiles(
                        new ZipWithPassword("password",
                            new KeyValuePair<string, IInput>("name1", new InputOf("ThisIsSparta")),
                            new KeyValuePair<string, IInput>("name2", new InputOf("ThisIsATest"))
                        ),
                        false
                    )
                ).Value()
            );
        }
    }
}
