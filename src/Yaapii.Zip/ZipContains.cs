using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Zip
{
    /// <summary>
    /// Checks whether a specific path exists in a zip or not.
    /// The path can either be a file or a directory.
    /// </summary>
    public sealed class ZipContains : IScalar<bool>
    {
        private readonly IInput zip;
        private readonly List<string> paths;
        private readonly bool leaveOpen;

        /// <summary>
        /// Checks whether a specific path exists in a zip or not.
        /// The path can either be a file or a directory.
        /// </summary>
        public ZipContains(IInput zip, IEnumerable<string> paths) : this(zip, true, paths)
        { }

        /// <summary>
        /// Checks whether a specific path exists in a zip or not.
        /// The path can either be a file or a directory.
        /// </summary>
        public ZipContains(IInput zip, params string[] paths) : this(zip, true, new ManyOf<string>(paths))
        { }

        /// <summary>
        /// Checks whether a specific path exists in a zip or not.
        /// The path can either be a file or a directory.
        /// </summary>
        public ZipContains(IInput zip, bool leaveOpen, params string[] paths) : this(zip, leaveOpen, new ManyOf<string>(paths))
        { }

        /// <summary>
        /// Checks whether a specific path exists in a zip or not.
        /// The path can either be a file or a directory.
        /// </summary>
        public ZipContains(IInput zip, bool leaveOpen, IEnumerable<string> paths)
        {
            this.zip = zip;
            this.paths = new List<string>(paths);
            this.leaveOpen = leaveOpen;
        }

        public bool Value()
        {
            lock (zip)
            {
                bool result = true;
                var existing =
                    new List<string>(
                        new Mapped<string, string>(
                            path => Normalized(path).ToLower(),
                            new ZipPaths(this.zip)
                        )
                    );

                foreach (var file in this.paths)
                {
                    if (!existing.Contains(Normalized(file)))
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            }
        }

        private string Normalized(string path)
        {
            return path.ToLower().Replace("\\", "/").TrimEnd('/');
        }
    }
}
