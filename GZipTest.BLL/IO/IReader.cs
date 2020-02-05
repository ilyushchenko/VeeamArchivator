using System;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public interface IReader : IDisposable
    {
        bool CanRead { get; }
        Block ReadNext();
    }
}
