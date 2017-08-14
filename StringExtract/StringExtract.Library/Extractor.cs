using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StringExtract.Library
{
    /// <summary>
    /// The extractor class, which contains the core logic of the program
    /// </summary>
    public class Extractor
    {
        // List of the extracted strings
        public List<string> Strings;

        /// <summary>
        /// The constructor for the extractor
        /// </summary>
        public Extractor()
        {
            Strings = new List<string>();
        }

        /// <summary>
        /// The main method that extracts the strings
        /// </summary>
        public void Extract(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    ParseStream(stream);

                    stream.Flush();
                }
            }
            else
            {
                throw new IOException($"File does not exist: {filePath}");
            }
        }

        /// <summary>
        /// Parses the input stream
        /// </summary>
        public void ParseStream(FileStream stream)
        {
            const int BUFFER_SIZE = 32768;

            byte[] buffer = new byte[BUFFER_SIZE];
            int bytesRead;

            do
            {
                bytesRead = stream.Read(buffer, 0, BUFFER_SIZE);
                if (bytesRead > 0)
                {
                    ProcessBuffer(buffer);
                }

            } while (bytesRead == BUFFER_SIZE);
        }

        /// <summary>
        /// Processes a buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void ProcessBuffer(byte[] buffer)
        {
            
        }
    }
}
