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
            //TODO: Validate
            Settings settings = null;

            try
            {
                settings = Settings.Parse(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(1);
            }

            using (var archivator = new GZipArchivator(settings))
            {
                archivator.Start();
            }
            
            Console.Read();
        }
    }
}