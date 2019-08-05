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
                            "password",
                            "text.txt",
                            new InputOf("safe")
                        ),
                        "text.txt",
                        "password"
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
                        "text.txt",
                        "password"
                    )
                ).AsString()
            );
        }
    }
}
