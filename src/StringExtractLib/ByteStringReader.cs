using System;
using System.Collections.Generic;

namespace StringExtractLib
{
    /// <summary>
    /// An <see cref="IStringReader"/> implementation used to extract strings from a byte array.
    /// </summary>
    public class ByteStringReader : IStringReader
    {
        /// <summary>
        /// The target byte array to extract strings from.
        /// </summary>
        public byte[] Source { get; private set; }

        /// <summary>
        /// The extraction options which the reader will follow.
        /// </summary>
        public StringReaderOptions Options { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ByteStringReader"/> for a given byte array source
        /// using default reader options.
        /// </summary>
        /// <param name="source">The byte array source.</param>
        public ByteStringReader(byte[] source)
        {
            Options = new FileStringReaderOptions();
            Source = source;
        }

        /// <summary>
        /// <summary>
        /// Creates a new <see cref="ByteStringReader"/> for a given byte array source
        /// using given reader options.
        /// </summary>
        /// <param name="source">The byte array source.</param>
        /// <param name="options">The string reader options.</param>
        public ByteStringReader(byte[] source, StringReaderOptions options)
        {
            Options = options;
            Source = source;
        }

        /// <summary>
        /// Reads and returns all strings from the target byte array using the string reader
        /// options set up while constructing the <see cref="ByteStringReader"/>.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll()
        {
            return ReadAll(Options);
        }

        /// <summary>
        /// Reads and returns all strings from the target byte array using the string reader
        /// options passed as a parameter.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            var processor = new BufferProcessor(options);

            return processor.ProcessBuffer(Source, Source.Length).Strings;
        }
    }
}
