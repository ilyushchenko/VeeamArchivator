using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GZipTest.BLL.Collections;
using GZipTest.BLL.IO;
using GZipTest.BLL.Models;

namespace GZipTest.BLL
{
    public class GZipArchivator
    {
        private readonly BlockingQueue<Block> _processingBuffer;
        private IReader _reader;
        private IWriter _writer;

        public GZipArchivator(string inputPath)
        {
            _reader = new FileReader(inputPath, Constants.DefaultBlockSize);
            _processingBuffer = new BlockingQueue<Block>();
        }

        public void Start()
        {
            var readThread = new Thread(Read) {Name = "Read thread"};
            readThread.Start();

            //TODO: Process blocks

            //TODO: Write blocks
        }

        private void Read()
        {
            while (_reader.CanRead)
            {
                var readedBlock = _reader.ReadNext();
                _processingBuffer.Enqueue(readedBlock);
            }
        }
    }
}
