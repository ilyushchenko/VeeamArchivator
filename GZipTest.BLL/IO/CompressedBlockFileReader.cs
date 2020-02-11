using System;
using System.IO;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public class CompressedBlockFileReader : IReader
    {
        private readonly Stream _fileStream;
        private int _blockId;

        public CompressedBlockFileReader(string path)
        {
            _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
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
            var sizeBuffer = new byte[sizeof(int)];
            _fileStream.Read(sizeBuffer, 0, sizeBuffer.Length);
            return BitConverter.ToInt32(sizeBuffer, 0);
        }
    }
}