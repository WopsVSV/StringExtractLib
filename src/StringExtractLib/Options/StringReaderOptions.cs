using System;

namespace StringExtractLib.Options
{
    public class StringReaderOptions
    {
        private int _minimumLength;
        private int? _maximumLength;

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

        public StringType SearchedStringType { get; set; }

        public StringReaderOptions()
        {
            MinimumLength = 1;
            MaximumLength = null;
            SearchedStringType = StringType.Both;
        }

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
