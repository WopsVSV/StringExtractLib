using StringExtractLib.Options;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StringExtractLib
{
    public class MemoryStringReader // : IStringReader
    {
        public MemoryStringReader()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Memory reading is only supported on Windows.");
            }
        }

        public IEnumerable<string> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ReadAll(StringReaderOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
