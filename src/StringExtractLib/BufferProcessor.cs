using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("StringExtractLib.Memory")]
namespace StringExtractLib
{
    internal class ProcessedBufferResult
    {
        public IList<string> Strings { get; private set; }
        public byte[]? ChunkRemainder { get; private set; }

        public ProcessedBufferResult(IList<string> strings, byte[]? chunkRemainder = null)
        {
            Strings = strings;
            ChunkRemainder = chunkRemainder;
        }
    }

    internal class BufferProcessor
    {
        private StringReaderOptions _options;
        private bool _singleChunk;
        private int _maxLength;
        private int _minLength;

        public BufferProcessor(StringReaderOptions options, bool singleChunk = true)
        {
            _options = options;
            _maxLength = options.MaximumLength.HasValue ? options.MaximumLength.Value : int.MaxValue;
            _minLength = options.MinimumLength;
            _singleChunk = singleChunk;
        }

        public ProcessedBufferResult ProcessBuffer(byte[] buffer, int bufferSize)
        {
            int offset = 0;
            bool isUtf16 = false;
            var strings = new List<string>();
            var outputString = string.Empty;

            while (offset + _options.MinimumLength < bufferSize)
            {
                var processed = ProcessString(buffer, bufferSize, offset, ref isUtf16, ref outputString, out var chunkRemainder);

                if (chunkRemainder?.Length > 0)
                {
                    return new ProcessedBufferResult(strings, chunkRemainder);
                }

                if (processed)
                {
                    offset += isUtf16 ? outputString.Length * 2 : outputString.Length;

                    if (_options.SearchedStringType == StringType.Utf8 && isUtf16)
                        continue;
                    if (_options.SearchedStringType == StringType.Utf16 && !isUtf16)
                        continue;

                    strings.Add(outputString);

                }
                else offset++;
            }

            return new ProcessedBufferResult(strings);
        }

        private bool ProcessString(byte[] buffer, int bufferSize, int offset, ref bool isUtf16, ref string outputString, out byte[]? chunkRemainder)
        {
            int i = 0;
            chunkRemainder = null;

            if (Table[buffer[offset]])
            {
                if (buffer[offset + 1] == 0x00)
                {
                    while (offset + i + 1 < bufferSize &&
                            Table[buffer[offset + i]] &&
                            buffer[offset + i + 1] == 0)
                    {

                        i += 2;
                    }

                    isUtf16 = true;
                }
                else
                {
                    while (offset + i < bufferSize && Table[buffer[offset + i]])
                        i++;

                    isUtf16 = false;
                }

                if (!_singleChunk && offset + i + 1 >= bufferSize)
                {
                    chunkRemainder = new byte[buffer.Length - offset];
                    Buffer.BlockCopy(buffer, offset, chunkRemainder, 0, buffer.Length - offset);

                    return false;
                }

                var len = isUtf16 ? i / 2 : i;
                if (len < _minLength)
                    return false;
                if (len > _maxLength)
                    return false;

                outputString = isUtf16
                    ? Encoding.Unicode.GetString(buffer, offset, i)
                    : Encoding.ASCII.GetString(buffer, offset, i);
                return true;
            }

            return false;
        }

        internal static readonly bool[] Table =
                /*          0     1     2     3        4     5     6     7        8     9     A     B        C     D     E     F     */
                /* 0x00 */ {false,false,false,false,   false,false,false,false,   false,true ,true ,false,   false,true ,false,false,
                /* 0x10 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0x20 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x30 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x40 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x50 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x60 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x70 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,false,
                /* 0x80 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0x90 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xA0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xB0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xC0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xD0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xE0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xF0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false};
    }
}
