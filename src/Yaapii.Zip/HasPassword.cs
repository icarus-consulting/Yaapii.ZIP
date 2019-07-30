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
            var stream = zip.Stream();
            stream.Seek(0, SeekOrigin.Begin);
            new FailWhen(
                () => !new IsZipArchive(this.zip).Value(),
                new ArgumentException(
                    "Can not extract zip because no zip was provided."
                )
            ).Go();
            stream.Seek(0, SeekOrigin.Begin);
            bool result;
            using (var zip = ZipFile.Read(this.zip.Stream()))
            {
                result = zip[virtualPath].UsesEncryption;
            }
            return result;
        }
    }
}
