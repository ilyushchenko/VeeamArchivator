using System.IO;
using System.IO.Compression;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.Processing
{
    public class Decompressor : IBlockProcessor
    {
        public Block Process(Block blockToCompress)
        {
            using (var sourceStream = new MemoryStream(blockToCompress.Data))
            using (var decompressedStream = new MemoryStream())
            using (var gZipDecompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
            {
                gZipDecompressionStream.CopyTo(decompressedStream);
                return new Block(blockToCompress.Id, decompressedStream.ToArray());
            }
        }
    }
}