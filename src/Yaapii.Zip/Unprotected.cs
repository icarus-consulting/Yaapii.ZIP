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
            var result = new Dictionary<string, IInput>();
            foreach (var file in new ZipFiles(origin, true))
            {
                if (new HasPassword(this.origin, file).Value())
                {
                    result.Add(file, new ZipPasswordExtracted(this.origin, file, this.password));
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
