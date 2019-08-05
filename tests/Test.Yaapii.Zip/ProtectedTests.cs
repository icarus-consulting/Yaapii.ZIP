using System.Collections.Generic;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ProtectedTests
    {
        [Fact]
        public void FileHasPassword()
        {
            Assert.True(
                new HasPassword(
                    new Protected("",
                        new Zipped(
                            new KeyValuePair<string, IInput>(
                                "file.exe", new InputOf("This is Sparta")
                            )
                        )
                    ), "file.exe"
                ).Value()
            );
        }

        [Fact]
        public void ProtectsMultipleFiles()
        {
            Assert.Equal(
                2,
                new Atoms.Enumerable.LengthOf(
                    new ZipFiles(
                        new Protected("Trojan",
                            new Zipped(
                                new KeyValuePair<string, IInput>("horse.file", new InputOf("not a horse")),
                                new KeyValuePair<string, IInput>("dog.cpp", new InputOf("cat"))
                            )
                        )
                    )
                ).Value()
            );
        }
    }
}
