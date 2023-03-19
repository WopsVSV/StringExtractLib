using System;

namespace StringExtractLib
{
    /// <summary>
    /// Represents a region of process memory with a fixed size.
    /// </summary>
    public class MemorySource
    {
        /// <summary>
        /// Handle of the process.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Base address of the memory region.
        /// </summary>
        public int Address { get; private set; }

        /// <summary>
        /// Length, in bytes, of the memory region.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Creates a new memory region representation from given parameters.
        /// </summary>
        /// <param name="handle">Handle of the process.</param>
        /// <param name="address">Base address of the memory region.</param>
        /// <param name="length">Length, in bytes, of the memory region.</param>
        /// <exception cref="ArgumentOutOfRangeException">Memory address or length is less than 0.</exception>
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
