using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Settings settings;
            try
            {
                settings = Settings.Parse(args);
            }
            catch (Exception e)
            {
                ShowErrorAndExit("Problem with input arguments", e);
                return 1; //For skip checking settings for null
            }

            try
            {
                using (var archivator = new GZipArchivator(settings))
                {
                    Console.WriteLine($"Start processing. Mode: {settings.Mode}");
                    var processingTime = Stopwatch.StartNew();
                    archivator.Start();
                    processingTime.Stop();
                    Console.WriteLine($"Done! Processed in {processingTime.Elapsed}");
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
            catch (InvalidDataException invalidDataException)
            {
                ShowErrorAndExit("Decompressor error", invalidDataException);
            }
            catch (Exception e)
            {
                ShowErrorAndExit("Unexpected error", e);
            }

            return 0;
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