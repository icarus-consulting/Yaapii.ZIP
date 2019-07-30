using Ionic.Zip;
using System.IO;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Zip
{
    /// <summary>
    /// A Zip which input is saved with a password
    /// </summary>
    public sealed class ZipWithPassword : IInput
    {
        private readonly IScalar<IInput> zip;

        /// <summary>
        /// A Zip which input is saved with a password
        /// </summary>
        public ZipWithPassword(string name, string password, IInput origin) : this(
            new Sticky<IInput>(() =>
            {
                IInput output;
                using (var stream = new MemoryStream())
                using (var zip = new ZipFile())
                {
                    zip.Password = password;
                    zip.AddEntry(name, origin.Stream());
                    zip.Save(stream);
                    output = new InputOf(stream.ToArray());
                }
                return output;
            }))
        { }

        private ZipWithPassword(IScalar<IInput> zip)
        {
            this.zip = zip;
        }

        public Stream Stream()
        {
            return zip.Value().Stream();
        }
    }
}
