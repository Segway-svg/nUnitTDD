using nUnitTDD.Excel;
using nUnitTDD.MockObjects;

namespace nUnitTDD.ParserObjects
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

        public ParseResult Parse(ExcelFile excelFile, bool validateCells = false)
        {
            var result = new ParseResult();
            result.ParsedRowsCount = excelFile.Rows.Count();
            result.IsFileParsed = true;

            if (excelFile.Rows.Any(x => x is InvalidRow))
            {
                _alertPublisher.SendAlert();
                result.HasInvalidRows = true;
                result.IsFileParsed = false;
            }

            if (validateCells)
            {
                foreach (var row in excelFile.Rows)
                {
                    if (!IsRowValid(row.Cells))
                    {
                        _alertPublisher.SendAlert();
                        result.IsFileParsed = false;
                        result.HasInvalidCells = true;
                    }
                }

                if (result.IsFileParsed)
                {
                    result.IsFileParsed = _excelManager.Save(excelFile);
                    result.WasSavedToStorage = result.IsFileParsed;
                }
            }

            return result;
        }

        public static bool IsRowValid(List<Cell> cells)
        {
            return cells.Count >= 3;
        }
    }
}