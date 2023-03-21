using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StringExtractLib
{
    /// <summary>
    /// An <see cref="IAsyncStringReader"/> implementation used to extract strings from a file.
    /// </summary>
    public class FileStringReader : IAsyncStringReader
    {
        /// <summary>
        /// The path of the target file to be extracted from.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The extraction options which the reader will follow.
        /// </summary>
        public FileStringReaderOptions Options { get; private set; }

        /// <summary>
        /// Creates a new <see cref="FileStringReader"/> for a given file
        /// using default string reader options.
        /// </summary>
        /// <param name="path">The file system path of the target file.</param>
        public FileStringReader(string path)
        {
            Options = new FileStringReaderOptions();
            Path = path;
        }

        /// <summary>
        /// Creates a new <see cref="FileStringReader"/> for a given file
        /// using specific string reader options.
        /// </summary>
        /// <param name="path">The file system path of the target file.</param>
        /// <param name="options">The file string reader options.</param>
        public FileStringReader(string path, FileStringReaderOptions options)
        {
            Options = options;
            Path = path;
        }

        /// <summary>
        /// Reads and returns all strings from the target file using the string reader
        /// options set up while constructing the <see cref="FileStringReader"/>.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll()
        {
            return ReadAll(Options);
        }

        /// <summary>
        /// Reads and returns all strings from the target file using the string reader
        /// options passed as a parameter.
        /// </summary>
        /// <returns>A list of all strings, filtered by the options.</returns>
        public IList<string> ReadAll(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            var fileReaderOptions = options is FileStringReaderOptions fileStringReaderOptions 
                ? fileStringReaderOptions : 
                new FileStringReaderOptions(options);

            var processor = new FileStringProcessor(Path, fileReaderOptions);
            return processor.ReadAll();
        }

        /// <inheritdoc/>
        public async Task<IList<string>> ReadAllAsync()
        {
            return await ReadAllAsync(Options);
        }

        /// <inheritdoc/>
        public async Task<IList<string>> ReadAllAsync(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            var fileReaderOptions = options is FileStringReaderOptions fileStringReaderOptions
                ? fileStringReaderOptions :
                new FileStringReaderOptions(options);

            var processor = new FileStringProcessor(Path, fileReaderOptions);
            return await processor.ReadAllAsync();
        }
    }
}