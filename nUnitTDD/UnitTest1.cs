using Moq;
using nUnitTDD.Excel.Files;
using nUnitTDD.Excel.FileStructure;
using nUnitTDD.MockObjects;
using nUnitTDD.ParseObjects;

namespace nUnitTDD
{
    public class UnitTest1
    {
        public Mock<IAlertPublisher> mockAlertPublisher = new Mock<IAlertPublisher>();
        public Mock<IExcelManager> mockExcelManager = new Mock<IExcelManager>();
        public Parser parser;

        private static ExcelFile CreateExcelFile(List<Row> rows)
        {
            return new ExcelFile(rows);
        }

        private static CsvFile CreateCsvFile(List<Row> rows)
        {
            return new CsvFile(rows);
        }

        [SetUp]
        public void Setup()
        {
            mockAlertPublisher.Reset();
            mockExcelManager.Reset();

            parser = new Parser(mockAlertPublisher.Object, mockExcelManager.Object);
        }

        [Test]
        public void ParserReturnsTwoAsParsedRowsCount()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(),
                new Row(),
            });

            var parseResult = parser.Parse(excelFile);


            Assert.That(parseResult.ParsedRowsCount, Is.EqualTo(2));
        }

        [Test]
        public void ParserReturnsThreeAsParsedRowsCount()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(),
                new Row(),
                new Row(),
            });

            var parseResult = parser.Parse(excelFile);

            Assert.That(parseResult.ParsedRowsCount, Is.EqualTo(3));
        }

        [Test]
        public void ParserSendAlertForInvalidRow()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new InvalidRow(),
            });

            var parsedResult = parser.Parse(excelFile);

            mockAlertPublisher.Verify(x => x.SendAlert());
        }

        [Test]
        public void ParserDoesNotSendAlertForNotInvalidRow()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()),
            });

            var parsedResult = parser.Parse(excelFile);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
        }

        [Test]
        public void ParserSendAlert_For_RowWithLowerThanThreeCells()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                }),
            });

            var parsedResult = parser.Parse(excelFile, true);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Once);
        }

        [Test]
        public void ParserDoesNotSendAlert_For_RowsWithNoLowerThanThreeCells()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("2"),
                }),
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("3"),
                    new Cell("4"),
                }),
            });

            var parsedResult = parser.Parse(excelFile);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
        }

        [Test]
        public void IfRowsParsed_AttemptProcessed_SaveToStorage()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("2"),
                }),
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("3"),
                    new Cell("4"),
                }),
            });

            mockExcelManager.Setup(x => x.Save(excelFile)).Returns(true);

            var parseResult = parser.Parse(excelFile, true);

            Assert.True(parseResult.IfFileParsed);
            Assert.True(parseResult.WasSavedToStorage);
        }

        [Test]
        public void IfRowsNotParsed_AttemptFailProcessed_DoNotSaveToStorage()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                }),
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                }),
            });

            mockExcelManager.Setup(x => x.Save(excelFile)).Returns(false);

            var parseResult = parser.Parse(excelFile, true);

            Assert.False(parseResult.IfFileParsed);
            Assert.False(parseResult.WasSavedToStorage);
        }

        [Test]
        public void IsCsvHasOneMoreCellThanExcelFileInRows()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                }),
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                }),
            });

            var csvFile = CreateCsvFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("3"),
                }),
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                    new Cell("3"),
                }),
            });

            mockExcelManager.Setup(x => x.Save(excelFile)).Returns(true);

            var parseResult = parser.Parse(excelFile, true);

            Assert.True(parser.CompareCsvAndExcel(csvFile, excelFile));

        }
    }
}