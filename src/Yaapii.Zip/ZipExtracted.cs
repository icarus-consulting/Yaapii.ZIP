using System;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// Content of a file in a zip archive
    /// is tolerant to slash style
    /// </summary>
    public sealed class ZipExtracted : IInput
    {
        private readonly IInput zip;
        private readonly string filePath;
        private readonly bool leaveOpen;

        /// <summary>
        /// Content of a file in a zip archive
        /// is tolerant to slash style
        /// </summary>
        public ZipExtracted(IInput zip, string virtualPath, bool leaveOpen = true)
        {
            this.zip = zip;
            this.filePath = virtualPath;
            this.leaveOpen = leaveOpen;
        }

        public Stream Stream()
        {
            lock (zip)
            {
                Stream content;
                using (var archive = new ZipArchive(this.zip.Stream(), ZipArchiveMode.Read, leaveOpen))
                {
                    var zipEntry =
                        new FirstOf<ZipArchiveEntry>(
                            new Filtered<ZipArchiveEntry>(entry =>
                                String.Compare(Normalized(entry.FullName), Normalized(this.filePath), true) == 0,
                                archive.Entries
                            ),
                            new ArgumentException($"Cannot extract file '{this.filePath}' because it doesn't exist in the zip archive. "
                                + $"These files exist: {Environment.NewLine}{string.Join(Environment.NewLine, new ZipPaths(this.zip))}")
                        ).Value();
                    content = Content(zipEntry);
                }
                content.Seek(0, SeekOrigin.Begin);
                content.Flush();
                return content;
            }
        }

        private Stream Content(ZipArchiveEntry zipEntry)
        {
            MemoryStream content = new MemoryStream();
            using (Stream zipEntryStream = zipEntry.Open())
            {
                zipEntryStream.CopyTo(content);
                zipEntryStream.Close();
                content.Seek(0, SeekOrigin.Begin);
            }
            return content;
        }

        private string Normalized(string path)
        {
            return path.Replace("\\", "/").TrimEnd('/');
        }
    }
}
