using System.IO;
using FEngLib.Chunks;

namespace FEngLib
{
    /// <summary>
    ///     Responsible for loading <see cref="FrontendPackage" /> instances
    /// </summary>
    public class FrontendPackageLoader
    {
        /// <summary>
        ///     Loads a data stream as a <see cref="FrontendPackage" />.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader" /> for the data stream.</param>
        /// <returns>A new <see cref="FrontendPackage" /> instance.</returns>
        public FrontendPackage Load(BinaryReader br)
        {
            var package = new FrontendPackage();
            var chunkReader = new FrontendChunkReader(package, br);

            foreach (var chunk in chunkReader.ReadMainChunks())
                switch (chunk)
                {
                    case PackageHeaderChunk packageHeaderChunk:
                        ProcessPackageHeaderChunk(package, packageHeaderChunk);
                        break;
                    case ObjectContainerChunk objectContainerChunk:
                        ProcessObjectContainerChunk(package, objectContainerChunk);
                        break;
                    case MessageDefinitionsChunk messageNamesChunk:
                        package.MessageDefinitions.AddRange(messageNamesChunk.Definitions);
                        break;
                }

            return package;
        }

        private void ProcessObjectContainerChunk(FrontendPackage package, ObjectContainerChunk objectContainerChunk)
        {
            package.Objects.AddRange(objectContainerChunk.Objects);
        }

        private void ProcessPackageHeaderChunk(FrontendPackage package, PackageHeaderChunk packageHeaderChunk)
        {
            package.Filename = packageHeaderChunk.Filename;
            package.Name = packageHeaderChunk.Name;
        }
    }
}