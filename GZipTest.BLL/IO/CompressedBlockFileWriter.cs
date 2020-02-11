using System;
using System.IO;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public class CompressedBlockFileWriter : IWriter
    {
        private readonly Stream _fileStream;

        public CompressedBlockFileWriter(string path)
        {
            _fileStream = new FileStream(path, FileMode.Create);
        }

        public void Write(Block blockToWrite)
        {
            var dataSize = BitConverter.GetBytes(blockToWrite.Data.Length);
            _fileStream.Write(dataSize, 0, dataSize.Length);
            _fileStream.Write(blockToWrite.Data, 0, blockToWrite.Data.Length);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }
    }
}