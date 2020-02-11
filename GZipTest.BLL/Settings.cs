using System;
using System.IO.Compression;

namespace GZipTest.BLL
{
    public class Settings
    {
        public Settings(string inputPath, string outputPath, CompressionMode mode)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            Mode = mode;
            BlockSize = Constants.DefaultBlockSize;
        }

        public string InputPath { get; }
        public string OutputPath { get; }
        public CompressionMode Mode { get; }
        public int BlockSize { get; set; }

        public static Settings Parse(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Wrong arguments count");
            }

            CompressionMode mode;
            switch (args[0].ToLower())
            {
                case "compress":
                    mode = CompressionMode.Compress;
                    break;
                case "decompress":
                    mode = CompressionMode.Decompress;
                    break;
                default:
                    throw new ArgumentException("The first parameter should be archivation mode(compress/decompress)");
            }
            return new Settings(args[1], args[2], mode);
        }
    }
}