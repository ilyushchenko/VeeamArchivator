using GZipTest.BLL.IO;
using GZipTest.BLL.Processing;

namespace GZipTest.BLL.Factories
{
    internal class DecompressorFactory : ArchivatorFactory
    {
        private readonly Settings _settings;

        public DecompressorFactory(Settings settings)
        {
            _settings = settings;
        }
        public override IReader GetReader()
        {
            return new CompressedBlockFileReader(_settings.InputPath);
        }

        public override IWriter GetWriter()
        {
            return new FileWriter(_settings.OutputPath);
        }

        public override IBlockProcessor GetBlockProcessor()
        {
            return new Decompressor();
        }
    }
}