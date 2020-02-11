using System.IO;
using System.IO.Compression;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.Processing
{
    public class Compressor : IBlockProcessor
    {
        public Block Process(Block blockToCompress)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var gZipCompression = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gZipCompression.Write(blockToCompress.Data, 0, blockToCompress.Data.Length);
                }

                var compressedBlock = new Block(blockToCompress.Id, compressedStream.ToArray());
                return compressedBlock;
            }
        }
    }
}