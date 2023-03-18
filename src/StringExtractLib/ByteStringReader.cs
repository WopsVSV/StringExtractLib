using System;
using System.Collections.Generic;

namespace StringExtractLib
{
    public class ByteStringReader : IStringReader
    {
        public byte[] Source { get; private set; }

        public StringReaderOptions Options { get; private set; }

        public ByteStringReader(byte[] source)
        {
            Options = new FileStringReaderOptions();
            Source = source;
        }

        public ByteStringReader(byte[] source, StringReaderOptions options)
        {
            Options = options;
            Source = source;
        }

        public IList<string> ReadAll()
        {
            return ReadAll(Options);
        }

        public IList<string> ReadAll(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            var processor = new BufferProcessor(options);

            return processor.ProcessBuffer(Source, Source.Length).Strings;
        }
    }
}
