using System;
using Yaapii.Atoms;
using Ionic.Zip;
using Yaapii.Atoms.Error;
using System.IO;

namespace Yaapii.Zip
{
    /// <summary>
    /// Checks if the file in a zip has a password
    /// </summary>
    public sealed class HasPassword : IScalar<bool>
    {
        private readonly IInput zip;
        private readonly string virtualPath;

        /// <summary>
        /// Checks if the file in a zip has a password
        /// </summary>
        public HasPassword(IInput zip, string virtualPath)
        {
            this.zip = zip;
            this.virtualPath = virtualPath;
        }

        public bool Value()
        {
            new FailWhen(
                () => !new IsZipArchive(this.zip).Value(),
                new ArgumentException("Cannot check for password because input is not a zip archive.")
            ).Go();

            // ZipFile.ContainsEntry() by Ionic does not work with backslashes,
            // even if the path is normalized. Misterious
            new FailWhen(
                () => !new ZipContains(this.zip, this.virtualPath).Value(),
                new ArgumentException($"Cannot check for password because the file '{this.virtualPath}' doesn't exist in the archive.")
            ).Go();

            bool result;
            var stream = this.zip.Stream();
            stream.Seek(0, SeekOrigin.Begin);
            using (var zip = ZipFile.Read(stream))
            {
                result = zip[this.virtualPath].UsesEncryption;
            }
            return result;
        }
    }
}
