using StringExtractLib.Options;
using System.Collections.Generic;
using System.Text;

namespace StringExtractLib
{
    internal class BufferProcessor
    {
        private StringReaderOptions _options;

        public BufferProcessor(StringReaderOptions options)
        {
            _options = options;
        }

        public IEnumerable<string> ProcessBuffer(byte[] buffer, int bufferSize)
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

        public int ProcessString(byte[] buffer, int bufferSize, int offset, ref int stringSize, ref string outputString)
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
