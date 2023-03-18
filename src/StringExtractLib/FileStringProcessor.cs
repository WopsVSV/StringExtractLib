using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace StringExtractLib
{
    internal class FileStringProcessor
    {
        private readonly BufferProcessor _bufferProcessor;
        private readonly FileStringReaderOptions _options;
        private readonly string _path;

        internal FileStringProcessor(string path, FileStringReaderOptions options)
        {
            _path = path;
            _options = options;
            _bufferProcessor = new BufferProcessor(options, !options.ChunkSize.HasValue);
        }

        internal IList<string> ReadAll()
        {
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return ParseStream(stream);
            }
        }

        private IList<string> ParseStream(FileStream stream)
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

        private IList<string> ProcessStream(FileStream stream)
        {
            var length = (int)stream.Length;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);

            return _bufferProcessor.ProcessBuffer(buffer, length).Strings;
        }

        private IList<string> ProcessChunkedStream(FileStream stream, int chunkSize)
        {
            var strings = new List<string>();
            byte[] buffer = new byte[chunkSize];
            int bufferSize;

            byte[]? chunkRemainder = Array.Empty<byte>();

            do
            {
                bufferSize = stream.Read(buffer, 0, chunkSize);

                if (bufferSize > 0)
                {
                    var targetArray = chunkRemainder != null && chunkRemainder.Length > 0
                        ? chunkRemainder.Concat(buffer).ToArray()
                        : buffer;

                    var processedBufferResult = _bufferProcessor.ProcessBuffer(targetArray, targetArray.Length);
                    chunkRemainder = processedBufferResult.ChunkRemainder;

                    strings.AddRange(processedBufferResult.Strings);
                }
            }
            while (bufferSize == chunkSize);

            return strings;
        } 
    }
}
