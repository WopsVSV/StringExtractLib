using System;

namespace StringExtractLib
{
    public class MemorySource
    {
        public IntPtr Handle { get; private set; }

        public int Address { get; private set; }

        public int Length { get; private set; }

        public MemorySource(IntPtr handle, int address, int length)
        {
            if (address < 0)
                throw new ArgumentOutOfRangeException(nameof(Address), "Memory source address must be greater than or equal to 0.");

            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(Length), "Memory source length must be greater than 0.");

            Handle = handle;
            Address = address;
            Length = length;
        }
    }
}
