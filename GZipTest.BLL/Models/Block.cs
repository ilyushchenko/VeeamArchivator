namespace GZipTest.BLL.Models
{
    public class Block
    {
        public Block(int id, byte[] data)
        {
            Id = id;
            Data = data;
        }

        public int Id { get; }
        public byte[] Data { get; }
    }
}
