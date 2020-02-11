using System;
using System.IO.Compression;
using GZipTest.BLL.IO;
using GZipTest.BLL.Processing;

namespace GZipTest.BLL.Factories
{
    internal abstract class ArchivatorFactory
    {
        public abstract IReader GetReader();
        public abstract IWriter GetWriter();
        public abstract IBlockProcessor GetBlockProcessor();
        public static ArchivatorFactory CreateFactory(Settings settings)
        {
            switch (settings.Mode)
            {
                case CompressionMode.Compress:
                    return new CompressorFactory(settings);
                case CompressionMode.Decompress:
                    return new DecompressorFactory(settings);
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings.Mode), settings.Mode,
                        "Unsupported archivation mode (try Compress or Decompress)");
            }
        }
    }
}