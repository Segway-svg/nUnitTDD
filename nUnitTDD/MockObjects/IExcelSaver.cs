using nUnitTDD.Excel;

namespace nUnitTDD.MockObjects
{
    public interface IExcelManager
    {
        public bool Save(ExcelFile excelFile);
    }
}
