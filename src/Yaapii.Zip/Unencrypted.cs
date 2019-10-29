using System;
using System.Collections.Generic;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Zip
{
    /// <summary>
    /// Decrypts a Zip archive
    /// </summary>
    public sealed class Decrypted : IInput
    {
        private readonly string password;
        private readonly IInput origin;

        /// <summary>
        /// Decrypts a Zip archive
        /// </summary>
        public Decrypted(string password, IInput origin)
        {
            this.password = password;
            this.origin = origin;
        }

        public Stream Stream()
        {
            new FailWhen(
                () => !new IsZipArchive(this.origin).Value(),
                new ArgumentException("Can not decrypt zip, because the input is not a zip archive")
            ).Go();
            var result = new Dictionary<string, IInput>();
            foreach (var file in new ZipFiles(origin, true))
            {
                if (new HasPassword(this.origin, file).Value())
                {
                    result.Add(file, new ZipDecryptedExtracted(this.origin, file, this.password));
                }
                else
                {
                    result.Add(file, new ZipExtracted(this.origin, file));
                }
            }
            return new Zipped(result).Stream();
        }
    }
}
