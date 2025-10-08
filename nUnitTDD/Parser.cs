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
            if (excelFile.Rows.Any(row => row is InvalidRow))
                _alertPublisher.SendAlert();

            return excelFile.Rows.Count();
        }

        public static bool IsRowValid(List<Row> rows)
        {
            return false;
        }
    }
}