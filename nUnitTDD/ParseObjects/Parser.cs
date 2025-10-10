using nUnitTDD.Excel.Files;
using nUnitTDD.Excel.FileStructure;
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

        public ParseResult Parse(IFile file, bool IsCheckCells = false)
        {
            var parseResult = new ParseResult();

            parseResult.ParsedRowsCount = file.Rows.Count();
            parseResult.IfFileParsed = true;

            if (file.Rows.Any(x => x is InvalidRow))
            {
                _alertPublisher.SendAlert();

                parseResult.HasInvalidRows = true;
                parseResult.ParsedRowsCount = file.Rows.Where(x => x is not InvalidRow).Count();
            }

            if (IsCheckCells)
            {
                foreach (var row in file.Rows)
                {
                    if (!IsRowValid(row.Cells))
                    {
                        row.IsValid = false;
                        _alertPublisher.SendAlert();
                        parseResult.HasInvalidCells = true;
                    }
                }
            }

            if (!parseResult.HasInvalidRows && !parseResult.HasInvalidCells)
            {
                _excelManager.Save(file);
                parseResult.WasSavedToStorage = true;
            }
            else
            {
                parseResult.IfFileParsed = false;
            }

            return parseResult;
        }

        public bool CompareCsvAndExcel(CsvFile csvFile, ExcelFile excelFile)
        {
            foreach (var csvRow in csvFile.Rows)
            {
                //if (!csvRow.IsValid || csvRow is InvalidRow)
                //    continue;

                foreach (var excelRow in excelFile.Rows)
                {
                    //if (!excelRow.IsValid || excelRow is InvalidRow)
                    //    continue;

                    if (csvRow.Cells.Count() - 1 != excelRow.Cells.Count())
                    {
                        return false;
                    }
                }
            }

            return true;
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