using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Zip.Test
{
    public sealed class UnprotectedTests
    {
        [Fact]
        public void FailsOnNoZip()
        {
            Assert.Throws<ArgumentException>(() =>
                new Unprotected("password", new InputOf("not a zip")).Stream()
            );
        }

        [Fact]
        public void FailsOnNoPassword()
        {
            Assert.Throws<ArgumentException>(() =>
                new Unprotected("password", new Zipped(new KeyValuePair<string, IInput>("name.file", new InputOf("content")))).Stream()
            );
        }

        [Fact]
        public void UnprotectsFile()
        {
            Assert.Equal("Eureka",
                new TextOf(
                    new ZipExtracted(
                        new Unprotected("pass",
                            new ZipWithPassword("pass",
                                new KeyValuePair<string, IInput>("path.exe", new InputOf("Eureka"))
                            )
                        ),
                        "path.exe"
                    )
                ).AsString()
            );
        }

        [Fact]
        public void UnprotectsMultipleFiles()
        {
            var files = new List<string>();
            var zip = new Unprotected("blacklist", new ZipWithPassword("blacklist", new KeyValuePair<string, IInput>("file1", new InputOf("remington")), new KeyValuePair<string, IInput>("file2", new InputOf("keen"))));
            foreach (var file in new ZipFiles(zip))
            {
                files.Add(new TextOf(new ZipExtracted(zip, file)).AsString());
            }
            Assert.Equal(2, files.Count);
        }
    }
}
