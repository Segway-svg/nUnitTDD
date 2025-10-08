using nUnitTDD.Excel;
using nUnitTDD.MockObjects;

namespace nUnitTDD
{
    public class Parser
    {
        public readonly IAlertPublisher _alertPublisher;
        public readonly IExcelManager _excelManager;

        public Parser(IAlertPublisher alertPublisher, IExcelManager excelManager)
        {
            _alertPublisher = alertPublisher;
            _excelManager = excelManager;
        }

        public int Parse(ExcelFile excelFile)
        {
            if (excelFile.Rows.Any(x => x is InvalidRow))
                _alertPublisher.SendAlert();
            
            return excelFile.Rows.Count();
        }

        public bool ParseWithCells(ExcelFile excelFile)
        {
            var isFileParsed = true;

            foreach (var row in excelFile.Rows)
            {
                if (!IsRowValid(row.Cells))
                {
                    _alertPublisher.SendAlert();
                    isFileParsed = false;
                }
            }

            if (isFileParsed)
            {
                isFileParsed = _excelManager.Save(excelFile);
            }

            return isFileParsed;
        }

        public static bool IsRowValid(List<Cell> cells)
        {    
            if (cells.Count() >= 3)
                return true;

            return false;
        }
    }
}