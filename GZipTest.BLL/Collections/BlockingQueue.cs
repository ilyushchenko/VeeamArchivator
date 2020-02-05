using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipTest.BLL.Collections
{
    public class BlockingQueue<T>
    {
        private readonly Queue<T> _queue;
        private bool _isClosed;

        public BlockingQueue()
        {
            _queue = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            if (_isClosed)
            {
                throw new InvalidOperationException("Trying to add item to closed queue");
            }

            lock (_queue)
            {
                _queue.Enqueue(item);
                Monitor.Pulse(_queue);
            }
        }

        public bool TryDequeue(out T item)
        {
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    if (_isClosed)
                    {
                        item = default(T);
                        return false;
                    }
                    Monitor.Wait(_queue);
                }

                item = _queue.Dequeue();
                return true;
            }
        }

        public void CloseQueue()
        {
            lock (_queue)
            {
                _isClosed = true;
                Monitor.PulseAll(_queue);
            }
        }
    }
}
