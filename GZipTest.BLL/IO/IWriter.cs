using System;
using GZipTest.BLL.Models;

namespace GZipTest.BLL.IO
{
    public interface IWriter : IDisposable
    {
        void Write(Block data);
    }
}
