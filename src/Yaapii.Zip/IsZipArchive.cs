using System;
using System.IO;
using Yaapii.Atoms;

namespace Yaapii.Zip
{
    /// <summary>
    /// Checks if the given data is a ZIP archive.
    /// </summary>
    public sealed class IsZipArchive : IScalar<bool>
    {
        private readonly IInput data;
        private const string SIGNATURE_EMPTY_GZIP = "50-4B-05-06";
        private const string SIGNATURE_ZIP = "50-4B-03-04";
        private const string SIGNATURE_GZIP = "1F-8B-08";

        /// <summary>
        /// Checks if the given data is a ZIP archive.
        /// </summary>
        public IsZipArchive(IInput data)
        {
            this.data = data;
        }

        public bool Value()
        {
            return
                MatchesSignature(this.data.Stream(), SIGNATURE_ZIP)
                || MatchesSignature(this.data.Stream(), SIGNATURE_GZIP)
                || MatchesSignature(this.data.Stream(), SIGNATURE_EMPTY_GZIP);
        }

        private bool MatchesSignature(Stream stream, string expected)
        {
            int expectedSize = expected.Split('-').Length;
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.Length < expectedSize)
                return false;

            byte[] signatureBytes = new byte[expectedSize];
            int required = expectedSize;
            int index = 0;
            while (required > 0)
            {
                int read = stream.Read(signatureBytes, index, required);
                required -= read;
                index += read;
            }
            string actualSignature = BitConverter.ToString(signatureBytes);
            if (actualSignature == expected) return true;
            return false;
        }
    }
}
