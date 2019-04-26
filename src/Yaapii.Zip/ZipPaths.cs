using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// The paths in a ZIP archive.
    /// Note: Extraction is sticky.
    /// </summary>
    public sealed class ZipPaths : IEnumerable<string>
    {
        private readonly IScalar<IEnumerable<string>> files;

        /// <summary>
        /// The paths in a ZIP archive.
        /// Note: Extraction is sticky.
        /// </summary>
        /// <param name="input"></param>
        public ZipPaths(IInput input, bool leaveOpen = true)
        {
            this.files =
                new SolidScalar<IEnumerable<string>>(() =>
                {
                    lock (input.Stream())
                    {
                        var inputStream = input.Stream();
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Flush();
                        IEnumerable<string> files = new EnumerableOf<string>();
                        using (var zip = new ZipArchive(inputStream, ZipArchiveMode.Read, leaveOpen))
                        {
                            if (zip.Entries.Count > 0)
                            {
                                files =
                                    new Mapped<ZipArchiveEntry, string>(entry =>
                                        entry.FullName,
                                        zip.Entries
                                    );
                            }
                        }
                        inputStream.Seek(0, SeekOrigin.Begin);
                        return files;
                    }
                });
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.files.Value().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
