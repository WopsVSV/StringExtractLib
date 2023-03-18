using StringExtractLib.Options;
using System.Collections.Generic;
using System.IO;
using System;

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
            _bufferProcessor = new BufferProcessor(options);
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

            return _bufferProcessor.ProcessBuffer(buffer, length);
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
                    strings.AddRange(_bufferProcessor.ProcessBuffer(buffer, bufferSize));
                }

            }
            while (bufferSize == chunkSize);

            return strings;
        } 
    }
}
