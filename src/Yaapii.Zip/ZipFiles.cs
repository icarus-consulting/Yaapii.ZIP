using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// The files in a ZIP archive.
    /// Note: Extraction is sticky.
    /// </summary>
    public class ZipFiles : IEnumerable<string>
    {
        private readonly IScalar<IEnumerable<string>> files;

        /// <summary>
        /// The paths in a ZIP archive.
        /// Note: Extraction is sticky.
        /// </summary>
        /// <param name="input"></param>
        public ZipFiles(IInput input, bool leaveOpen = true)
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
                                    new Mapped<ZipArchiveEntry, string>(filtered =>
                                        filtered.FullName,
                                        new Filtered<ZipArchiveEntry>(entry =>
                                            !entry.FullName.EndsWith("/"),
                                            zip.Entries
                                        )
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
