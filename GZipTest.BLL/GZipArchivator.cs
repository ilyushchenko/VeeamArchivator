using System;
using System.Collections.Generic;
using System.Threading;
using GZipTest.BLL.Collections;
using GZipTest.BLL.IO;
using GZipTest.BLL.Models;
using GZipTest.BLL.Processing;

namespace GZipTest.BLL
{
    public class GZipArchivator : IDisposable
    {
        private readonly BlockingQueue<Block> _processingBuffer;
        private readonly IReader _reader;
        private readonly IWriter _writer;
        private readonly BlockingDictionary<int, Block> _writingBuffer;

        public GZipArchivator(string inputPath, string outputPath)
        {
            //TODO: inject processing mode and factory
            _reader = new FileReader(inputPath, Constants.DefaultBlockSize);
            _writer = new CompressedBlockFileWriter(outputPath);
            _processingBuffer = new BlockingQueue<Block>();
            _writingBuffer = new BlockingDictionary<int, Block>();
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _writer?.Dispose();
        }

        public void Start()
        {
            var readThread = new Thread(Read) {Name = "Read thread"};
            readThread.Start();

            var processingThreads = new List<Thread>();

            for (var i = 0; i < Environment.ProcessorCount + 2; i++)
            {
                var processingThread = new Thread(Processing) {Name = $"Processing {i}"};
                processingThreads.Add(processingThread);
                processingThread.Start();
            }

            var writeThread = new Thread(Write) {Name = "Write thread"};
            writeThread.Start();

            readThread.Join();
            _processingBuffer.CloseQueue();

            foreach (var processingThread in processingThreads) processingThread.Join();
            _writingBuffer.Close();

            writeThread.Join();
        }

        #region Producer / Consumer Parts Processing

        private void Read()
        {
            while (_reader.CanRead)
            {
                var readedBlock = _reader.ReadNext();
                _processingBuffer.Enqueue(readedBlock);
            }
        }

        private void Processing()
        {
            while (_processingBuffer.TryDequeue(out var blockToProcess))
            {
                var compressor = new Compressor();
                var processedBlock = compressor.Process(blockToProcess);

                _writingBuffer.Add(processedBlock.Id, processedBlock);
            }
        }

        private void Write()
        {
            var blockIdToWrite = 0;
            while (_writingBuffer.TryRemoveValue(blockIdToWrite, out var block))
            {
                _writer.Write(block);
                blockIdToWrite++;
            }
        }

        #endregion
    }
}