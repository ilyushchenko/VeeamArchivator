using GZipTest.BLL.Models;

namespace GZipTest.BLL.Processing
{
    public interface IBlockProcessor
    {
        Block Process(Block blockToCompress);
    }
}
