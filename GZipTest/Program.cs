using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Settings settings = null;
            try
            {
                settings = Settings.Parse(args);
            }
            catch (Exception e)
            {
                ShowErrorAndExit("Problem with input arguments", e);
            }

            try
            {
                using (var archivator = new GZipArchivator(settings))
                {
                    archivator.Start();
                }
            }
            catch (OutOfMemoryException memoryException)
            {
                ShowErrorAndExit("The application needs more memory", memoryException);
            }
            catch (IOException ioException)
            {
                ShowErrorAndExit("Input or Output file error", ioException);
            }
            catch (UnauthorizedAccessException accessException)
            {
                ShowErrorAndExit("Unable to access file", accessException);
            }
            catch (Exception e)
            {
                ShowErrorAndExit("Unexpected error", e);
            }

            Console.Read();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowErrorAndExit("Unexpected error", (Exception)e.ExceptionObject);
        }

        private static void ShowErrorAndExit(string message, Exception e)
        {
            Console.WriteLine(message);
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}