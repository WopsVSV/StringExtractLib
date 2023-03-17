using System;

namespace StringExtractLib.Options
{
    public class FileStringReaderOptions : StringReaderOptions
    {
        private int? _chunkSize;

        public int? ChunkSize
        {
            get
            {
                return _chunkSize;
            }
            set
            {
                if (value.HasValue && value.Value < 1)
                    throw new ArgumentOutOfRangeException(nameof(ChunkSize), "ChunkSize must be greater than or equal to 1.");

                if (value.HasValue && value.Value <= MaximumLength)
                    throw new ArgumentOutOfRangeException(nameof(ChunkSize), "ChunkSize must be greater than MaximumLength.");

                _chunkSize = value;
            }
        }

        public FileStringReaderOptions() : base()
        {
            ChunkSize = 4096;
        }

        public FileStringReaderOptions(StringReaderOptions options) : base()
        {
            MinimumLength = options.MinimumLength;
            MaximumLength = options.MaximumLength;
            SearchedStringType = options.SearchedStringType;
        }

        public FileStringReaderOptions(
            int minimumSize = 1, 
            int? maximumSize = null, 
            StringType stringType = StringType.Both, 
            int? chunkSize = 4096)
            : base(minimumSize, maximumSize, stringType)
        {
            ChunkSize = chunkSize;
        }
    }
}
