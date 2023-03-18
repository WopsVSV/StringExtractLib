using StringExtractLib.Options;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StringExtractLib
{
    public class MemoryStringReader : IStringReader
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesRead);

        public MemorySource Source { get; private set; }

        public StringReaderOptions Options { get; private set; }

        public MemoryStringReader(MemorySource source)
        {
            Source = source;
            Options = new StringReaderOptions();
        }

        public MemoryStringReader(MemorySource source, StringReaderOptions options)
        {
            Source = source;
            Options = options;
        }

        public IEnumerable<string> ReadAll()
        {
            return ReadAll(Options);
        }

        public IEnumerable<string> ReadAll(StringReaderOptions options)
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
            return byteProcessor.ProcessBuffer(buffer, buffer.Length);
        }
    }
}