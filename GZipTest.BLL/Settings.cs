using System;
using System.IO;
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
            BlocksLimit = Constants.DefaultBlocksLimit;
        }

        public string InputPath { get; }
        public string OutputPath { get; }
        public CompressionMode Mode { get; }
        public int BlockSize { get; set; }
        public int BlocksLimit { get; }

        public static Settings Parse(string[] args)
        {
            if (args?.Length != 3)
            {
                throw new ArgumentException("Wrong arguments count. Usage: GZipTest.exe [compress/decompress] [input path] [output path]");
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

            if (args[1] == args[2])
            {
                throw new Exception("Input and output file must be different!");
            }

            var inputFileInfo = new FileInfo(args[1]);
            if (!inputFileInfo.Exists)
            {
                throw new Exception("Invalid second argument. File not found!");
            }
            if (inputFileInfo.Length == 0)
            {
                throw new Exception("Invalid second argument. File empty!");
            }

            var outputFileInfo = new FileInfo(args[2]);
            if (!Directory.Exists(outputFileInfo.DirectoryName))
            {
                throw new Exception("Invalid third argument. Output directory not found!");
            }
            if (outputFileInfo.Exists)
            {
                throw new Exception("Invalid third argument. File already exists!");
            }

            return new Settings(args[1], args[2], mode);
        }
    }
}