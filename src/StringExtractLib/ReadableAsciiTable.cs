using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringExtractLib
{
    internal static class ReadableAsciiTable
    {
        internal static readonly bool[] Table =
                /*          0     1     2     3        4     5     6     7        8     9     A     B        C     D     E     F     */
                /* 0x00 */ {false,false,false,false,   false,false,false,false,   false,true ,true ,false,   false,true ,false,false,
                /* 0x10 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0x20 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x30 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x40 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x50 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x60 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
                /* 0x70 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,false,
                /* 0x80 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0x90 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xA0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xB0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xC0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xD0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xE0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
                /* 0xF0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false};
    }
}

