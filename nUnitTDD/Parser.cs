using nUnitTDD.Excel;

namespace nUnitTDD
{
    public class Parser
    {
        public readonly IAlertPublisher _alertPublisher;

        public Parser(IAlertPublisher alertPublisher)
        {
            _alertPublisher = alertPublisher;
        }

        public int Parse(ExcelFile excelFile)
        {
            if (excelFile.Rows.Any(x => x is InvalidRow))
                _alertPublisher.SendAlert();
            
            return excelFile.Rows.Count();
        }

        public int ParseWithCells(ExcelFile excelFile)
        {
            foreach (var row in excelFile.Rows)
            {
                if (!IsRowValid(row.Cells))
                    _alertPublisher.SendAlert();
            }

            return excelFile.Rows.Count();
        }

        public static bool IsRowValid(List<Cell> cells)
        {    
            if (cells.Count() >= 3)
                return true;

            return false;
        }
    }
}