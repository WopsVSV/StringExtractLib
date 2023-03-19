namespace StringExtractLib
{
    /// <summary>
    /// Represents the string type search option for a <see cref="IStringReader"/>,
    /// indicating what kind of strings should be extracted from the data source.
    /// </summary>
    public enum StringType
    {
        /// <summary>
        /// Both UTF8 and UTF16 strings.
        /// </summary>
        Both = 0,

        /// <summary>
        /// UTF8 strings only.
        /// </summary>
        Utf8 = 1,

        /// <summary>
        /// UTF16 strings only.
        /// </summary>
        Utf16 = 2
    }
}
