using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// The files in a ZIP archive.
    /// Note: Extraction is sticky.
    /// </summary>
    public sealed class ZipFiles : IEnumerable<string>
    {
        private readonly IScalar<IEnumerable<string>> files;

        /// <summary>
        /// The files in a ZIP archive.
        /// Note: Extraction is sticky.
        /// </summary>
        /// <param name="input"></param>
        public ZipFiles(IInput input, bool leaveOpen = true)
        {
            this.files =
                new Solid<IEnumerable<string>>(()=>
                    new Filtered<string>(path => 
                        !path.EndsWith("/"), 
                        new ZipPaths(input)
                    )
                );
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.files.Value().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
