using nUnitTDD.Excel.FileStructure;

namespace nUnitTDD.Excel.Files
{
    public class CsvFile : IFile
    {
        public List<Row> Rows { get; }

        public CsvFile(List<Row> rows)
        {
            Rows = rows;
        }
    }
}