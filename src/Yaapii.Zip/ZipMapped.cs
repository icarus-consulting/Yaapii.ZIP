using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// A zip input mapped from a given zip input
    /// Maps the zip entry paths according to the given mapping function
    /// </summary>
    public sealed class ZipMapped : IInput
    {
        private readonly IScalar<Stream> input;

        /// <summary>
        /// A zip input mapped from a given zip input
        /// Maps the zip entry paths according to the given mapping function
        /// </summary>
        public ZipMapped(Func<string, string> mapPath, IInput zip, bool leaveOpen = true)
        {
            this.input =
                new StickyScalar<Stream>(() =>
                {
                    lock (zip)
                    {
                        Stream inMemory = zip.Stream();
                        inMemory.Seek(0, SeekOrigin.Begin);
                        var newMemory = new MemoryStream();

                        using (var archive = new ZipArchive(inMemory, ZipArchiveMode.Read, leaveOpen))
                        using (var newArchive = new ZipArchive(newMemory, ZipArchiveMode.Create, leaveOpen))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                Move(entry, newArchive, mapPath);
                            }
                            inMemory.Position = 0;
                        }
                        newMemory.Seek(0, SeekOrigin.Begin);
                        newMemory.Flush();
                        return newMemory;
                    }
                });
        }
        public Stream Stream()
        {
            return this.input.Value();
        }

        private void Move(ZipArchiveEntry source, ZipArchive archive, Func<string, string> mapping)
        {
            var mapped = mapping(source.FullName);
            using (var sourceStream = source.Open())
            using (var stream = archive.CreateEntry(mapped).Open())
            {
                new LengthOf(
                    new TeeInput(
                        new InputOf(sourceStream),
                        new OutputTo(stream)
                    )
                ).Value();
            }
        }
    }
}
