using System;
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
                    new Encrypted("",
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
                        new Encrypted("Trojan",
                            new Zipped(
                                new KeyValuePair<string, IInput>("horse.file", new InputOf("not a horse")),
                                new KeyValuePair<string, IInput>("dog.cpp", new InputOf("cat"))
                            )
                        )
                    )
                ).Value()
            );
        }

        [Fact]
        public void FailsOnNoZip()
        {
            Assert.Throws<ArgumentException>(() =>
                new Encrypted("irrelevant", new InputOf("not a zip")).Stream()
            );
        }
    }
}
