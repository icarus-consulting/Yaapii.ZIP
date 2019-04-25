using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Zip
{
    /// <summary>
    /// Contents zipped.
    /// </summary>
    public sealed class Zipped : IInput
    {
        private readonly IEnumerable<KeyValuePair<string, IInput>> contents;

        /// <summary>
        /// Contents zipped.
        /// </summary>
        /// <param name="contents">contents in format 'virtualPath=data'</param>
        public Zipped(string path, IInput content) : this(new KeyValuePair<string, IInput>(path, content))
        { }

        /// <summary>
        /// Contents zipped.
        /// </summary>
        /// <param name="contents">contents in format 'virtualPath=data'</param>
        public Zipped(params KeyValuePair<string, IInput>[] contents) : this(new EnumerableOf<KeyValuePair<string, IInput>>(contents))
        { }

        /// <summary>
        /// Contents zipped.
        /// </summary>
        /// <param name="contents">contents in format 'virtualPath=data'</param>
        public Zipped(IEnumerable<KeyValuePair<string, IInput>> contents)
        {
            this.contents = contents;
        }

        public Stream Stream()
        {
            lock (this.contents)
            {
                var stream = new MemoryStream();
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
                {
                    foreach (var entry in this.contents)
                    {
                        var content = entry.Value;
                        using (var fileContent = content.Stream())
                        using (var archiveContent = zip.CreateEntry(entry.Key).Open())
                        {
                            fileContent.CopyTo(archiveContent);
                        }
                    }
                }
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
    }
}
