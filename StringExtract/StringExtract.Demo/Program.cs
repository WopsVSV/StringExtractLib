using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StringExtract.Library;

namespace StringExtract.Demo
{
    /// <summary>
    /// Startup object for the demo of StringExtract.
    /// </summary>
    public class Program
    {
        // The dummy program name
        private const string DUMMY_PATH = "StringExtract.Dummy.exe";

        /// <summary>
        /// Entry point of the program.
        /// </summary>
        /// <param name="args">The arguments passed through the command line.</param>
        public static void Main(string[] args)
        {
            // Creates a new extractor
            var extractor = new Extractor(5);

            // Extracts the strings
            //extractor.Extract(DUMMY_PATH);

            // Prints out the output
            foreach (var value in extractor.Extract(DUMMY_PATH))
                Console.WriteLine(value);

            // Await input before exit
            Console.ReadKey(true);
        }
    }
}
