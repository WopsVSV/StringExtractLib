using System.Collections.Generic;

namespace StringExtractLib
{
    /// <summary>
    /// Provides members for reading strings from a data source.
    /// </summary>
    public interface IStringReader
    {
        /// <summary>
        /// Reads all strings synchronously and returns them in a list.
        /// </summary>
        /// <returns>The list of strings.</returns>
        IList<string> ReadAll();

        /// <summary>
        /// Reads all strings synchronously using a given set of reading options, and returns them in a list.
        /// </summary>
        /// <param name="options">Options for the reader.</param>
        /// <returns>The list of strings.</returns>
        IList<string> ReadAll(StringReaderOptions options);
    }
}
