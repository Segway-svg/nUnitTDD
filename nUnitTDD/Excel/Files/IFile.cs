using nUnitTDD.Excel.FileStructure;

namespace nUnitTDD.Excel.Files
{
    public interface IFile
    {
        List<Row> Rows { get; }
    }
}