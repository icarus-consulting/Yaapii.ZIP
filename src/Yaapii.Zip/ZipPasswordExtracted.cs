﻿using Ionic.Zip;
using System;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.IO;

namespace Yaapii.Zip
{
    /// <summary>
    /// Extracted file in a zip that ís protected with a password
    /// </summary>
    public sealed class ZipPasswordExtracted : IInput
    {
        private readonly IInput zip;
        private readonly string virtualPath;
        private readonly string password;

        /// <summary>
        /// Extracted file in a zip that ís protected with a password
        /// </summary>
        public ZipPasswordExtracted(IInput zip, string virtualPath, string password)
        {
            this.zip = zip;
            this.virtualPath = virtualPath;
            this.password = password;
        }

        public Stream Stream()
        {
            new FailWhen(
                () => !new IsZipArchive(this.zip).Value(),
                new ArgumentException(
                    "Can not extract zip because no zip was provided."
                )
            ).Go();
            new FailWhen(
                () => !new HasPassword(this.zip, this.virtualPath).Value(),
                new InvalidOperationException(
                    "Can not extract zip because the file is not protected with a password."
                )
            ).Go();
            zip.Stream().Seek(0, SeekOrigin.Begin);
            IInput result;
            using (var stream = new MemoryStream())
            using (var zip = ZipFile.Read(this.zip.Stream()))
            {
                try
                {
                    zip[virtualPath].ExtractWithPassword(stream, this.password);
                }
                catch (BadPasswordException)
                {
                    throw new ArgumentException("Can not extract zip because the provided password is not correct.");
                }
                result = new InputOf(stream.ToArray());
            }
            return result.Stream();
        }
    }
}