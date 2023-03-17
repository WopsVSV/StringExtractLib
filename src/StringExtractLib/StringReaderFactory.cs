using System;
using System.Runtime.InteropServices;

namespace StringExtractLib
{
    public static class StringReaderFactory
    {
        public static FileStringReader FromFile(string path)
        {
            return new FileStringReader(path);
        }

        public static MemoryStringReader FromMemory(IntPtr memoryPtr, int size)
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

        public static ByteStringReader FromBytes(byte[] bytes)
        {
            return new ByteStringReader();
        }
    }
}
