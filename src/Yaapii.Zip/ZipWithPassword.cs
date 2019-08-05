using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// A Zip which input is saved with a password
    /// </summary>
    public sealed class ZipWithPassword : IInput
    {
        private readonly Sticky<IInput> result;

        /// <summary>
        /// A Zip which input is saved with a password
        /// </summary>
        public ZipWithPassword(string password, string name, IInput content) : this(
            password,
            new KeyValuePair<string, IInput>(
                name,
                content
            )
        )
        { }



        /// <summary>
        /// A Zip which input is saved with a password
        /// </summary>
        public ZipWithPassword(string password, params KeyValuePair<string, IInput>[] contents) : this(
            password,
            new EnumerableOf<KeyValuePair<string, IInput>>(
                contents
            )
        )
        { }

        /// <summary>
        /// A Zip which input is saved with a password
        /// </summary>
        public ZipWithPassword(string password, IEnumerable<KeyValuePair<string, IInput>> contents)
        {
            this.result = new Sticky<IInput>(() =>
            {
                var stream = new MemoryStream();
                using (var zip = new ZipFile())
                {
                    zip.Password = password;
                    foreach (var entry in contents)
                    {
                        var content = entry.Value;
                        zip.AddEntry(entry.Key, content.Stream());
                    }
                    zip.Save(stream);
                }
                stream.Seek(0, SeekOrigin.Begin);
                return new InputOf(stream.ToArray());
            });
        }

        public Stream Stream()
        {
            return this.result.Value().Stream();
        }
    }
}
