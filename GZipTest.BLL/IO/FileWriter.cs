using System.IO;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public class FileWriter : IWriter
    {
        private readonly Stream _fileStream;

        public FileWriter(string path)
        {
            _fileStream = new FileStream(path, FileMode.Create);
        }

        public void Write(Block blockToWrite)
        {
            _fileStream.Write(blockToWrite.Data, 0, blockToWrite.Data.Length);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }
    }
}