using System.Collections.Generic;
using System.Threading.Tasks;

namespace StringExtractLib
{
    /// <summary>
    /// Provides members for reading strings from a data source asynchronously.
    /// </summary>
    public interface IAsyncStringReader : IStringReader
    {
        /// <summary>
        /// Reads all strings asynchronously and returns them in a list.
        /// </summary>
        /// <returns>The list of strings.</returns>
        Task<IList<string>> ReadAllAsync();

        /// <summary>
        /// Reads all strings asynchronously using a given set of reading options, and returns them in a list.
        /// </summary>
        /// <param name="options">Options for the reader.</param>
        /// <returns>The list of strings.</returns>
        Task<IList<string>> ReadAllAsync(StringReaderOptions options);
    }
}
