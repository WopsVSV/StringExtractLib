using System;

namespace StringExtractLib
{
    /// <summary>
    /// Represents the string inclusion parameters that a <see cref="IStringReader"/> should follow
    /// when extracting strings from a given data source.
    /// </summary>
    public class StringReaderOptions
    {
        private int _minimumLength;
        private int? _maximumLength;

        /// <summary>
        /// The minimum (inclusive) length of an extracted string. 
        /// This value must be greater than or equal to 1.
        /// Default: 1
        /// </summary>
        public int MinimumLength
        {
            get 
            { 
                return _minimumLength; 
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(MinimumLength), "MinimumLength must be greater than or equal to 1.");

                _minimumLength = value;
            }
        }

        /// <summary>
        /// The maximum (inclusive) length of an extracted string.
        /// This value must be greater than or equal to the <see cref="MinimumLength"/>.
        /// Default: <see cref="null"/> - no upper bound.
        /// </summary>
        public int? MaximumLength
        {
            get
            {
                return _maximumLength;
            }
            set
            {
                if (value.HasValue && value.Value < MinimumLength)
                    throw new ArgumentOutOfRangeException(nameof(MaximumLength), "MaximumLength must be greater than or equal to MinimumLength.");

                _maximumLength = value;
            }
        }

        /// <summary>
        /// The type of strings that should be returned by the reader.
        /// Default: <see cref="StringType.Both"/> - all strings will be extracted.
        /// </summary>
        public StringType SearchedStringType { get; set; }

        /// <summary>
        /// Creates a <see cref="StringReaderOptions"/> with default values.
        /// </summary>
        public StringReaderOptions()
        {
            MinimumLength = 1;
            MaximumLength = null;
            SearchedStringType = StringType.Both;
        }

        /// <summary>
        /// Creates string reader options based on the given constructor parameters.
        /// </summary>
        /// <param name="minimumLength">Minimum length of the extracted strings.</param>
        /// <param name="maximumLength">Maximum length of the extracted strings.</param>
        /// <param name="stringType">Searched string type of the extracted strings.</param>
        public StringReaderOptions(
            int minimumLength = 1, 
            int? maximumLength = null, 
            StringType stringType = StringType.Both)
        {
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
            SearchedStringType = stringType;
        }
    }
}
