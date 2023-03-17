using System.Collections.Generic;

namespace StringExtractLib
{
    public interface IStringReader
    {
        IEnumerable<string> ReadAll();
    }
}
