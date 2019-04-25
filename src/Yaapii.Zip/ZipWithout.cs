using System;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// A zip from which a file has been removed.
    /// </summary>
    public sealed class ZipWithout : IInput
    {
        private readonly IScalar<Stream> zip;

        /// <summary>
        /// A zip from which a file has been removed.
        /// </summary>
        public ZipWithout(IInput input, string pathToRemove, bool leaveOpen = true) : this(
            new ScalarOf<Stream>(
                () => input.Stream()), 
                pathToRemove, 
                leaveOpen
            )
        { }

        /// <summary>
        /// A zip from which a file has been removed.
        /// </summary>
        public ZipWithout(IScalar<Stream> zip, string pathToRemove, bool leaveOpen)
        {
            this.zip = new StickyScalar<Stream>(() =>
            {
                lock (zip.Value())
                {
                    var stream = zip.Value();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen))
                    {
                        new ItemAt<ZipArchiveEntry>(
                            new Filtered<ZipArchiveEntry>(entry =>
                                String.Compare(Normalized(entry.FullName), Normalized(pathToRemove), true) == 0,
                                archive.Entries
                            ),
                            new ArgumentException(
                                $"Cannot remove file '{Normalized(pathToRemove)}' because it doesn't exist in the zip archive. "
                                + $"These files exist:{Environment.NewLine}"
                                + String.Join(
                                    $"{Environment.NewLine}",
                                    new Sorted<string>(
                                        new Mapped<ZipArchiveEntry, string>(
                                            entry => entry.FullName,
                                            archive.Entries
                                        )
                                    )
                                )
                            )
                        )
                        .Value()
                        .Delete();
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Flush();
                    return stream;
                }
            });
        }

        public Stream Stream()
        {
            return this.zip.Value();
        }

        private string Normalized(string path)
        {
            var dir = Path.GetDirectoryName(path).ToLower();
            var file = Path.GetFileName(path).ToLower();
            return Path.Combine(dir, file);
        }
    }
}
