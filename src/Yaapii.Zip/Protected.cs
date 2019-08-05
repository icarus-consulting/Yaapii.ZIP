using System;
using System.Collections.Generic;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Zip
{
    /// <summary>
    /// Takes a ZipArchive and protects each file with a password
    /// </summary>
    public sealed class Protected : IInput
    {
        private readonly string password;
        private readonly IInput origin;

        /// <summary>
        /// Takes a ZipArchive and protects each file with a password
        /// </summary>
        public Protected(string password, IInput origin)
        {
            this.password = password;
            this.origin = origin;
        }

        public Stream Stream()
        {
            new FailWhen(
                () => !new IsZipArchive(this.origin).Value(),
                new ArgumentException("Can not Protect zip, because the input is no ZipArchive")
            ).Go();
            var result = new Dictionary<string, IInput>();
            foreach (var file in new ZipFiles(origin, true))
            {
                result.Add(file, new ZipExtracted(origin, file, true));
            }
            return new ZipWithPassword(this.password, result).Stream();
        }
    }
}
