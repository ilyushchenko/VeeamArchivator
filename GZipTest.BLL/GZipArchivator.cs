using System;
using System.Collections.Generic;
using System.Threading;
using GZipTest.BLL.Collections;
using GZipTest.BLL.Factories;
using GZipTest.BLL.IO;
using GZipTest.BLL.Models;
using GZipTest.BLL.Processing;

namespace GZipTest.BLL
{
    public class GZipArchivator : IDisposable
    {
        private readonly Semaphore _blocksLimiter;
        private readonly BlockingQueue<Block> _processingBuffer;
        private readonly IBlockProcessor _processor;
        private readonly IReader _reader;
        private readonly IWriter _writer;
        private readonly BlockingDictionary<int, Block> _writingBuffer;
        private Exception _error;

        public GZipArchivator(Settings settings)
        {
            var factory = ArchivatorFactory.CreateFactory(settings);
            _reader = factory.GetReader();
            _writer = factory.GetWriter();
            _processor = factory.GetBlockProcessor();

            _processingBuffer = new BlockingQueue<Block>();
            _writingBuffer = new BlockingDictionary<int, Block>();
            _blocksLimiter = new Semaphore(settings.BlocksLimit, settings.BlocksLimit);
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _writer?.Dispose();
            _blocksLimiter?.Dispose();
        }

        public void Start()
        {
            var readThread = new Thread(Read) {Name = "Read thread"};
            readThread.Start();

            var processingThreads = new List<Thread>();

            for (var i = 0; i < Environment.ProcessorCount; i++)
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

            if (_error != null) throw _error;
        }

        #region Producer / Consumer Parts Processing

        private void Read()
        {
            try
            {
                while (_reader.CanRead && _error == null)
                {
                    _blocksLimiter.WaitOne();
                    var readedBlock = _reader.ReadNext();
                    _processingBuffer.Enqueue(readedBlock);
                }
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        private void Processing()
        {
            try
            {
                while (_processingBuffer.TryDequeue(out var blockToProcess) && _error == null)
                {
                    var processedBlock = _processor.Process(blockToProcess);
                    _writingBuffer.Add(processedBlock.Id, processedBlock);
                }
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        private void Write()
        {
            var blockIdToWrite = 0;
            try
            {
                while (_writingBuffer.TryRemoveValue(blockIdToWrite, out var block) && _error == null)
                {
                    _writer.Write(block);
                    blockIdToWrite++;
                    _blocksLimiter.Release();
                }
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        #endregion
    }
}