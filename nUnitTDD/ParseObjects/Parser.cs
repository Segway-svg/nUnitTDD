using nUnitTDD.Excel;
using nUnitTDD.MockObjects;

namespace nUnitTDD.ParseObjects
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

        public ParseResult Parse(ExcelFile excelFile, bool IsCheckCells = false)
        {
            var parseResult = new ParseResult();

            parseResult.ParsedRowsCount = excelFile.Rows.Count();
            parseResult.IfFileParsed = true;

            if (excelFile.Rows.Any(x => x is InvalidRow))
            {
                _alertPublisher.SendAlert();

                parseResult.HasInvalidRows = true;
                parseResult.ParsedRowsCount = excelFile.Rows.Where(x => x is not InvalidRow).Count();
            }

            if (IsCheckCells)
            {
                foreach (var row in excelFile.Rows)
                {
                    if (!IsRowValid(row.Cells))
                    {
                        _alertPublisher.SendAlert();
                        parseResult.HasInvalidCells = true;
                    }
                }
            }

            if (!parseResult.HasInvalidRows && !parseResult.HasInvalidCells)
            {
                _excelManager.Save(excelFile);
                parseResult.WasSavedToStorage = true;
            }
            else
            {
                parseResult.IfFileParsed = false;
            }

            return parseResult;
        }

        public bool SaveToStorage(ExcelFile excelFile)
        {
            return _excelManager.Save(excelFile);
        }

        public static bool IsRowValid(List<Cell> cells)
        {
            return cells.Count() >= 3;
        }
    }
}