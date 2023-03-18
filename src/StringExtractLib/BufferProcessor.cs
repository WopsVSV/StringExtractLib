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

        public BufferProcessor(StringReaderOptions options, bool singleChunk = true)
        {
            _options = options;
            _singleChunk = singleChunk;
        }

        public ProcessedBufferResult ProcessBuffer(byte[] buffer, int bufferSize)
        {
            int offset = 0;
            int stringSize = 0;
            var strings = new List<string>();

            while (offset + _options.MinimumLength < bufferSize)
            {
                var outputString = string.Empty;
                int stringDiskSpace = ProcessString(buffer, bufferSize, offset, ref stringSize, ref outputString, out var chunkRemainder);

                if (chunkRemainder?.Length > 0)
                {
                    return new ProcessedBufferResult(strings, chunkRemainder);
                }

                if (stringSize >= _options.MinimumLength)
                {
                    offset += stringDiskSpace;

                    bool isUtf16 = stringDiskSpace > stringSize;
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

        private int ProcessString(byte[] buffer, int bufferSize, int offset, ref int stringSize, ref string outputString, out byte[]? chunkRemainder)
        {
            int i = 0;
            chunkRemainder = null;
            StringBuilder builder = new StringBuilder();

            if (ReadableAsciiTable.Table[buffer[offset]])
            {
                if (buffer[offset + 1] == 0x00)
                {
                    while (offset + i + 1 < bufferSize &&
                            ReadableAsciiTable.Table[buffer[offset + i]] &&
                            buffer[offset + i + 1] == 0)
                    {
                        if (_options.MaximumLength.HasValue)
                        {
                            if (i / 2 + 1 > _options.MaximumLength)
                                break;
                        }

                        builder.Append((char)buffer[offset + i]);
                        i += 2;
                    }

                    if (!_singleChunk && offset + i + 1 >= bufferSize)
                    {
                        chunkRemainder = new byte[buffer.Length - offset];
                        Buffer.BlockCopy(buffer, offset, chunkRemainder, 0, buffer.Length - offset);

                        stringSize = 0;
                        return 0;
                    }

                    outputString = builder.ToString();
                    stringSize = i / 2;
                    return i;
                }
                else
                {
                    i = offset;

                    while (i < bufferSize && ReadableAsciiTable.Table[buffer[i]])
                        i++;

                    stringSize = i - offset;

                    if (_options.MaximumLength.HasValue && stringSize > _options.MaximumLength.Value)
                        stringSize = _options.MaximumLength.Value;

                    outputString = Encoding.ASCII.GetString(buffer, offset, stringSize);
                    return stringSize;
                }
            }

            stringSize = 0;
            return 0;
        }
    }
}
