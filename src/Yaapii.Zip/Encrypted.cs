using System;
using System.Collections.Generic;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Zip
{
    /// <summary>
    /// A zip archive, encrypted.
    /// </summary>
    public sealed class Encrypted : IInput
    {
        private readonly string password;
        private readonly IInput origin;

        /// <summary>
        /// A zip archive, encrypted.
        /// </summary>
        public Encrypted(string password, IInput origin)
        {
            this.password = password;
            this.origin = origin;
        }

        public Stream Stream()
        {
            new FailWhen(
                () => !new IsZipArchive(this.origin).Value(),
                new ArgumentException("Cannot encrypt zip, because the input is not a zip archive.")
            ).Go();
            var result = new Dictionary<string, IInput>();
            foreach (var file in new ZipFiles(origin, true))
            {
                result.Add(file, new ZipExtracted(origin, file, true));
            }
            return new ZipEncrypted(this.password, result).Stream();
        }
    }
}
