using System.IO;
using FEngLib.Chunks;

namespace FEngLib.Packages
{
    /// <summary>
    ///     Responsible for loading <see cref="Package" /> instances
    /// </summary>
    public class FrontendPackageLoader
    {
        /// <summary>
        ///     Loads a data stream as a <see cref="Package" />.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader" /> for the data stream.</param>
        /// <returns>A new <see cref="Package" /> instance.</returns>
        public Package Load(BinaryReader br)
        {
            var package = new Package();
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

        private void ProcessObjectContainerChunk(Package package, ObjectContainerChunk objectContainerChunk)
        {
            package.Objects.AddRange(objectContainerChunk.Objects);
        }

        private void ProcessPackageHeaderChunk(Package package, PackageHeaderChunk packageHeaderChunk)
        {
            package.Filename = packageHeaderChunk.Filename;
            package.Name = packageHeaderChunk.Name;
        }
    }
}