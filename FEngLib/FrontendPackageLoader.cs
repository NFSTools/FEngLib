using System.Diagnostics;
using System.IO;

namespace FEngLib
{
    /// <summary>
    /// Responsible for loading <see cref="FrontendPackage"/> instances
    /// </summary>
    public class FrontendPackageLoader
    {
        /// <summary>
        /// Loads a data stream as a <see cref="FrontendPackage"/>.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> for the data stream.</param>
        /// <returns>A new <see cref="FrontendPackage"/> instance.</returns>
        public FrontendPackage Load(BinaryReader br)
        {
            FrontendPackage package = new FrontendPackage();
            FrontendChunkReader chunkReader = new FrontendChunkReader(package, br);

            foreach (var chunk in chunkReader.ReadMainChunks())
            {
                Debug.WriteLine(chunk);
            }

            return package;
        }
    }
}