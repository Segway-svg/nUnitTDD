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

        public bool IsRowValid(Row row)
        {
            return row.Cells.Count() == 4;
        }
    }
}