using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Zip
{
    public sealed class Protected : IInput
    {
        private readonly string password;
        private readonly IInput origin;

        public Protected(string password, IInput origin)
        {
            this.password = password;
            this.origin = origin;
        }

        public Stream Stream()
        {
            var result = new List<KeyValuePair<string, IInput>>();
            foreach (var file in new ZipFiles(origin, true))
            {
                result.Add(new KeyValuePair<string, IInput>(file, new ZipExtracted(origin, file, true)));
            }
            return new ZipWithPassword(this.password, result).Stream();
        }
    }
}
