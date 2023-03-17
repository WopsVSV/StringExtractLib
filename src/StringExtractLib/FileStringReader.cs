using StringExtractLib.Options;
using System;
using System.Collections.Generic;

namespace StringExtractLib
{
    public class FileStringReader : IStringReader
    {
        private FileStringReaderOptions _options;
        private string _path;

        public FileStringReaderOptions Options { get; private set; }

        public FileStringReader(string path)
        {
            Options = new FileStringReaderOptions();

            _path = path;
            _options = Options;
        }

        public FileStringReader(string path, FileStringReaderOptions options)
        {
            Options = options;

            _path = path;
            _options = Options;
        }

        public IEnumerable<string> ReadAll()
        {
            return ReadAll(_options);
        }

        public IEnumerable<string> ReadAll(StringReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(StringReaderOptions), "StringReaderOptions cannot be null.");

            var fileReaderOptions = options is FileStringReaderOptions fileStringReaderOptions 
                ? fileStringReaderOptions : 
                new FileStringReaderOptions(options);

            var processor = new FileStringProcessor(_path, fileReaderOptions);
            return processor.ReadAll();
        }
    }
}