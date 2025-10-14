using nUnitTDD.Excel.FileStructure;

namespace nUnitTDD.Excel.Files
{
    public class ExcelFile : IFile
    {
        public List<Row> Rows {  get; }

        public ExcelFile(List<Row> rows)
        {
            Rows = rows;
        }

        public bool IsRowValid(Row row)
        {
            return row.Cells.Count() == 3;
        }
    }
}
