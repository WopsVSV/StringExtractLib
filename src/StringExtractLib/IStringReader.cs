using StringExtractLib.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StringExtractLib
{
    public interface IStringReader
    {
        IEnumerable<string> ReadAll();

        IEnumerable<string> ReadAll(StringReaderOptions options);
    }
}
