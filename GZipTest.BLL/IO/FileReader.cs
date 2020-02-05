using System.IO;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public class FileReader : IReader
    {
        private int _blockId;
        private readonly int _blockSize;
        private readonly Stream _fileStream;

        public FileReader(string path, int blockSize)
        {
            _blockSize = blockSize;
            _fileStream = new FileStream(path, FileMode.Open);
        }

        public bool CanRead => _fileStream.Position < _fileStream.Length;

        public Block ReadNext()
        {
            var bufferSize = GetBufferSize();
            var buffer = new byte[bufferSize];

            _fileStream.Read(buffer, 0, buffer.Length);

            return new Block(_blockId++, buffer);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }

        private int GetBufferSize()
        {
            var remainingBytes = _fileStream.Length - _fileStream.Position;
            return remainingBytes > _blockSize ? _blockSize : (int)remainingBytes;
        }
    }
}