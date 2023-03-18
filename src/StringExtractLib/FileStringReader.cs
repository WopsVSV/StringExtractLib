using System;
using System.Collections.Generic;

namespace StringExtractLib
{
    public class FileStringReader : IStringReader
    {
        public string Path { get; private set; }

        public FileStringReaderOptions Options { get; private set; }

        public FileStringReader(string path)
        {
            Options = new FileStringReaderOptions();
            Path = path;
        }

        public FileStringReader(string path, FileStringReaderOptions options)
        {
            Options = options;
            Path = path;
        }

        public IList<string> ReadAll()
        {
            return ReadAll(Options);
        }

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
    }
}