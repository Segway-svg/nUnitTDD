using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nUnitTDD.ParserObjects
{
    public class ParseResult
    {
        public int ParsedRowsCount { get; set; }
        public bool IsFileParsed { get; set; }
        public bool HasInvalidRows { get; set; }
        public bool HasInvalidCells { get; set; }
        public bool WasSavedToStorage { get; set; }
    }
}
