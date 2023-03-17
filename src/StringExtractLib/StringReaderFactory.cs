using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StringExtractLib
{
    public static class StringReaderFactory
    {
        public static IStringReader FromFile(string path)
        {
            return new FileStringReader();
        }

        public static IStringReader FromMemory(IntPtr memoryPtr, int size)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new MemoryStringReader();
            }
            else
            {
                throw new NotSupportedException("Memory reading is only supported on Windows.");
            }
        }

        public static IStringReader FromBytes(byte[] bytes)
        {
            return new ByteStringReader();
        }
    }
}
