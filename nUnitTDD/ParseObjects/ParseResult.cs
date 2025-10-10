namespace nUnitTDD.ParseObjects
{
    public class ParseResult
    {
        public int ParsedRowsCount { get; set; }
        public bool HasInvalidRows { get; set; }
        public bool HasInvalidCells { get; set; }
        public bool IfFileParsed { get; set; }
        public bool WasSavedToStorage { get; set; }
    }
}