using System;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// A zip from which a file has been updated or added.
    /// If the file to update does not exist, it is created.
    /// </summary>
    public sealed class ZipUpdated : IInput
    {
        private readonly IScalar<Stream> zip;

        /// <summary>
        /// A zip in which a file has been updated or added.
        /// If the file to update does not exist, it is created.
        /// </summary>
        public ZipUpdated(IInput input, string pathToUpdate, IInput update, bool leaveOpen = true) : this(
            new ScalarOf<Stream>(() => input.Stream()), pathToUpdate, update, leaveOpen
        )
        { }

        /// <summary>
        /// A zip from which a file has been updated or added.
        /// If the file to update does not exist, it is created.
        /// </summary>
        public ZipUpdated(IScalar<Stream> zip, string pathToUpdate, IInput update, bool leaveOpen)
        {
            this.zip = new Solid<Stream>(() =>
            {
                new FailWhen(
                    () => ExistsAndCrypted(new InputOf(zip.Value()), pathToUpdate),
                    new InvalidOperationException(
                        $"Cannot update '{pathToUpdate}' because the file is password protected"
                    )
                ).Go();

                lock (zip.Value())
                {
                    var stream = zip.Value();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen))
                    {
                        foreach (var entry in
                            new Filtered<ZipArchiveEntry>(entry =>
                                String.Compare(Normalized(entry.FullName), Normalized(pathToUpdate), true) == 0,
                                archive.Entries
                            )
                        )
                        {
                            entry.Delete();
                            break;
                        }

                        using (var entry = archive.CreateEntry(Normalized(pathToUpdate)).Open())
                        {
                            var data = update.Stream();
                            data.CopyTo(entry);
                        }
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Flush();
                    return stream;
                }
            },
            zip);
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

        private bool ExistsAndCrypted(IInput zip, string filepath)
        {
            bool result = false;
            if(new ZipContains(zip, filepath).Value())
            {
                if(new HasPassword(zip, filepath).Value())
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
