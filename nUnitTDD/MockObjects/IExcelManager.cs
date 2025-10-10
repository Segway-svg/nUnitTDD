using nUnitTDD.Excel.Files;

namespace nUnitTDD.MockObjects
{
    public interface IExcelManager
    {
        public bool Save(IFile excelFile);
    }
}
