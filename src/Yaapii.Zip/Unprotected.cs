using System;
using System.Collections.Generic;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Zip
{
    /// <summary>
    /// Takes a ZipArchive and unprotects each file
    /// </summary>
    public sealed class Unprotected : IInput
    {
        private readonly string password;
        private readonly IInput origin;

        /// <summary>
        /// Takes a ZipArchive and unprotects each file
        /// </summary>
        public Unprotected(string password, IInput origin)
        {
            this.password = password;
            this.origin = origin;
        }

        public Stream Stream()
        {
            new FailWhen(
                () => !new IsZipArchive(this.origin).Value(),
                new ArgumentException("Can not unprotect zip, because the input is not a ZipArchive")
            ).Go();
            var result = new List<KeyValuePair<string, IInput>>();
            foreach (var file in new ZipFiles(origin, true))
            {
                new FailWhen(
                    () => !new HasPassword(
                        this.origin,
                        file
                    ).Value(),
                    new ArgumentException($"Can not unprotect file because file: {file} has no password")
                ).Go();
                result.Add(new KeyValuePair<string, IInput>(file, new ZipPasswordExtracted(this.origin, file, this.password)));
            }
            return new Zipped(result).Stream();
        }
    }
}
