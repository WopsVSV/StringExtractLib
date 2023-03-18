using System.Collections.Generic;

namespace StringExtractLib
{
    public interface IStringReader
    {
        IList<string> ReadAll();

        IList<string> ReadAll(StringReaderOptions options);
    }
}
