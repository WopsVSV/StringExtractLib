using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace StringExtractLib
{
    internal partial class FileStringProcessor
    {
        internal async Task<IList<string>> ReadAllAsync()
        {
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return await ParseStreamAsync(stream);
            }
        }

        private async Task<IList<string>> ParseStreamAsync(FileStream stream)
        {
            if (_options.ChunkSize.HasValue)
            {
                return await ProcessChunkedStreamAsync(stream, _options.ChunkSize.Value);
            }
            else
            {
                if (stream.Length > int.MaxValue)
                {
                    throw new InvalidOperationException("Unable to read file without chunking due to memory limitations.");
                }

                return await ProcessStreamAsync(stream);
            }
        }

        private async Task<IList<string>> ProcessStreamAsync(FileStream stream)
        {
            var length = (int)stream.Length;
            byte[] buffer = new byte[length];
            await stream.ReadAsync(buffer, 0, length);

            return _bufferProcessor.ProcessBuffer(buffer, length).Strings;
        }

        private async Task<IList<string>> ProcessChunkedStreamAsync(FileStream stream, int chunkSize)
        {
            var strings = new List<string>();
            byte[] buffer = new byte[chunkSize];
            int bufferSize;

            byte[]? chunkRemainder = Array.Empty<byte>();

            do
            {
                bufferSize = await stream.ReadAsync(buffer, 0, chunkSize);

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
