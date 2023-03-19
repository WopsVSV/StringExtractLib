using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StringExtractLib
{
    /// <summary>
    /// An <see cref="IStringReader"/> implementation used to extract strings from a memory region.
    /// </summary>
    public class MemoryStringReader : IStringReader
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesRead);

        /// <summary>
        /// The memory region to extract strings from.
        /// </summary>
        public MemorySource Source { get; private set; }

        /// <summary>
        /// The extraction options which the reader will follow.
        /// </summary>
        public StringReaderOptions Options { get; private set; }

        /// <summary>
        /// Creates a new <see cref="MemoryStringReader"/> for a given memory source
        /// using default string reader options.
        /// </summary>
        /// <param name="source">The target memory source.</param>
        public MemoryStringReader(MemorySource source)
        {
            Source = source;
            Options = new StringReaderOptions();
        }

        /// <summary>
        /// Creates a new <see cref="MemoryStringReader"/> for a given memory source
        /// using specific string reader options.
        /// </summary>
        /// <param name="source">The target memory source.</param>
        /// <param name="options">The string reader options.</param>
        public MemoryStringReader(MemorySource source, StringReaderOptions options)
        {
            Source = source;
            Options = options;
        }

        /// <summary>
        /// Reads and returns all strings from the target memory region using the string reader
        /// options set up while constructing the <see cref="MemoryStringReader"/>.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll()
        {
            return ReadAll(Options);
        }

        /// <summary>
        /// Reads and returns all strings from the target memory region using the string reader
        /// options passed as a parameter.
        /// Note: Reading from memory using <see cref="MemoryStringReader"/> is only possible on Windows platforms.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException("MemoryStringReader can only be used on Windows operating systems.");

            byte[] buffer = new byte[Source.Length];

            try
            {
                ReadProcessMemory((int)Source.Handle, Source.Address, buffer, buffer.Length, out _);
            }
            catch
            {
                throw new ApplicationException("An error occured while trying to read process memory.");
            }

            var byteProcessor = new BufferProcessor(Options);
            return byteProcessor.ProcessBuffer(buffer, buffer.Length).Strings;
        }
    }
}