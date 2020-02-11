using System.Collections.Generic;
using System.Threading;

namespace GZipTest.BLL.Collections
{
    public class BlockingDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private bool _isClosed;

        public BlockingDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue item)
        {
            lock (_dictionary)
            {
                _dictionary[key] = item;
                Monitor.Pulse(_dictionary);
            }
        }

        public bool TryRemoveValue(TKey key, out TValue item)
        {
            lock (_dictionary)
            {
                while (!_dictionary.ContainsKey(key))
                {
                    if (_isClosed)
                    {
                        item = default(TValue);
                        return false;
                    }

                    Monitor.Wait(_dictionary);
                }


                item = _dictionary[key];
                _dictionary.Remove(key);
                return true;
            }
        }

        public void Close()
        {
            lock (_dictionary)
            {
                _isClosed = true;
                Monitor.PulseAll(_dictionary);
            }
        }
    }
}