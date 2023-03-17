using StringExtractLib.Options;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace StringExtractLib
{
    internal class FileStringProcessor
    {
        private readonly FileStringReaderOptions _options;
        private readonly string _path;

        internal FileStringProcessor(string path, FileStringReaderOptions options)
        {
            _path = path;
            _options = options;
        }

        internal IEnumerable<string> ReadAll()
        {
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return ParseStream(stream);
            }
        }

        private IEnumerable<string> ParseStream(FileStream stream)
        {
            if (_options.ChunkSize.HasValue)
            {
                return ProcessChunkedStream(stream, _options.ChunkSize.Value);
            }
            else
            {
                if (stream.Length > int.MaxValue)
                {
                    throw new InvalidOperationException("Unable to read file without chunking due to memory limitations.");
                }

                return ProcessStream(stream);
            }
        }

        private IEnumerable<string> ProcessStream(FileStream stream)
        {
            var length = (int)stream.Length;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);

            return ProcessBuffer(buffer, length);
        }

        private IEnumerable<string> ProcessChunkedStream(FileStream stream, int chunkSize)
        {
            var strings = new List<string>();
            byte[] buffer = new byte[chunkSize];
            int bufferSize;

            do
            {
                bufferSize = stream.Read(buffer, 0, chunkSize);
                if (bufferSize > 0)
                {
                    strings.AddRange(ProcessBuffer(buffer, bufferSize));
                }

            }
            while (bufferSize == chunkSize);

            return strings;
        } 

        /// <summary>
        /// Processes a buffer
        /// </summary>
        private IEnumerable<string> ProcessBuffer(byte[] buffer, int bufferSize)
        {
            int offset = 0;
            int stringSize = 0;

            while (offset + _options.MinimumLength < bufferSize)
            {
                var outputString = string.Empty;
                int stringDiskSpace = ProcessString(buffer, bufferSize, offset, ref stringSize, ref outputString);

                if (stringSize >= _options.MinimumLength)
                {
                    offset += stringDiskSpace;

                    bool isUtf16 = stringDiskSpace > stringSize;
                    if (_options.SearchedStringType == StringType.Utf8 && isUtf16)
                        continue;
                    if (_options.SearchedStringType == StringType.Utf16 && !isUtf16)
                        continue;

                    yield return outputString;
   
                }
                else offset++;
            }
        }

        private int ProcessString(byte[] buffer, int bufferSize, int offset, ref int stringSize, ref string outputString)
        {
            int i = 0;
            StringBuilder builder = new StringBuilder();

            // Try to parse as ascii or unicode

            if (ReadableAsciiTable.Table[buffer[offset]])
            {
                // Consider unicode case
                if (buffer[offset + 1] == 0x00) // No null dereference by assumptions
                {
                    // Parse as unicode
                    while (offset + i + 1 < bufferSize && // We don't go out of the buffer
                            ReadableAsciiTable.Table[buffer[offset + i]] && // The char is readable ascii
                            buffer[offset + i + 1] == 0) // Next character is 0
                    {
                        if (_options.MaximumLength.HasValue)
                        {
                            if (i / 2 + 1 > _options.MaximumLength)
                                break;
                        }

                        builder.Append((char)buffer[offset + i]);
                        i += 2;
                    }

                    outputString = builder.ToString();
                    stringSize = i / 2;
                    return i;
                }
                else
                {
                    // Parse as ASCII
                    i = offset;

                    // As long as the next byte is a valid ASCII char
                    while (i < bufferSize && ReadableAsciiTable.Table[buffer[i]])
                        i++;

                    stringSize = i - offset;

                    if (_options.MaximumLength.HasValue && stringSize > _options.MaximumLength.Value)
                        stringSize = _options.MaximumLength.Value;

                    // Copy this string to the output
                    outputString = Encoding.ASCII.GetString(buffer, offset, stringSize);
                    return stringSize;
                }
            }

            stringSize = 0;
            return 0;
        }
    }
}
