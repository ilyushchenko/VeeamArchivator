using GZipTest.BLL.IO;
using GZipTest.BLL.Processing;

namespace GZipTest.BLL.Factories
{
    internal class CompressorFactory : ArchivatorFactory
    {
        private readonly Settings _settings;

        public CompressorFactory(Settings settings)
        {
            _settings = settings;
        }

        public override IReader GetReader()
        {
            return new FileReader(_settings.InputPath, _settings.BlockSize);
        }

        public override IWriter GetWriter()
        {
            return new CompressedBlockFileWriter(_settings.OutputPath);
        }

        public override IBlockProcessor GetBlockProcessor()
        {
            return new Compressor();
        }
    }
}