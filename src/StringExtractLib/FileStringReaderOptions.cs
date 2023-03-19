using System;

namespace StringExtractLib
{
    /// <summary>
    /// Represents the string inclusion parameters that a <see cref="FileStringReader"/> should follow
    /// when extracting strings from a given file source.
    /// </summary>
    public class FileStringReaderOptions : StringReaderOptions
    {
        private int? _chunkSize;

        /// <summary>
        /// The chunk size, in bytes, in which the file will be read. A <see cref="null"/> value indicates no chunking shall be performed.
        /// This value must be greater than or equal to 1 and less than the <see cref="StringReaderOptions.MaximumLength"/>.
        /// Default: 4096
        /// </summary>
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

        /// <summary>
        /// Creates a <see cref="FileStringReaderOptions"/> object with default values.
        /// </summary>
        public FileStringReaderOptions() : base()
        {
            ChunkSize = 4096;
        }

        /// <summary>
        /// Creates a <see cref="FileStringReaderOptions"/> object with default values and the base values
        /// inherited from the provided base <see cref="StringReaderOptions"/>.
        /// </summary>
        /// <param name="options">The base string reader options.</param>
        public FileStringReaderOptions(StringReaderOptions options) : this()
        {
            MinimumLength = options.MinimumLength;
            MaximumLength = options.MaximumLength;
            SearchedStringType = options.SearchedStringType;
        }

        /// <summary>
        /// Creates a <see cref="FileStringReaderOptions"/> object  based on the given constructor parameters.
        /// </summary>
        /// <param name="minimumLength">Minimum length of the extracted strings.</param>
        /// <param name="maximumLength">Maximum length of the extracted strings.</param>
        /// <param name="stringType">Searched string type of the extracted strings.</param>
        /// <param name="chunkSize">The chunk size in which the file will be read.</param>
        public FileStringReaderOptions(
            int minimumLength = 1, 
            int? maximumLength = null, 
            StringType stringType = StringType.Both, 
            int? chunkSize = 4096)
            : base(minimumLength, maximumLength, stringType)
        {
            ChunkSize = chunkSize;
        }
    }
}
