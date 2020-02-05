using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GZipTest.BLL;
using GZipTest.BLL.Collections;

namespace GZipTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Wrong arguments count");
                Environment.Exit(1);
            }

            //TODO: Validate
            var mode = args[0];
            var inputPath = args[1];
            var outputPath = args[2];

            var archivator = new GZipArchivator(inputPath);
            archivator.Start();

            Console.Read();
        }
    }
}