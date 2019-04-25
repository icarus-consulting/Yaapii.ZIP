using System.IO.Compression;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip.Test
{
    public sealed class ZipMappedTests
    {
        [Fact]
        public void Maps()
        {
            var mapped =
                new ZipMapped(
                    path => "directory/content",
                    new Zipped("original path/content", new InputOf("Map me if you can"))
                );

            using (var zip = new ZipArchive(mapped.Stream(), ZipArchiveMode.Read, false))
            {
                Assert.Contains(
                    "directory/content",
                    new ItemAt<ZipArchiveEntry>(
                        zip.Entries
                    ).Value().FullName
                );
            }
        }
    }
}
