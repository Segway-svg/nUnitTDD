using Moq;
using nUnitTDD.Excel;
using nUnitTDD.MockObjects;
using nUnitTDD.ParserObjects;

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

            var result = parser.Parse(excelFile);

            Assert.That(result.ParsedRowsCount, Is.EqualTo(2));
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

            var result = parser.Parse(excelFile);

            Assert.That(result.ParsedRowsCount, Is.EqualTo(3));
        }

        [Test]
        public void ParserSendAlertForInvalidRow()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new InvalidRow(),
            });

            var result = parser.Parse(excelFile, validateCells: true);

            mockAlertPublisher.Verify(x => x.SendAlert());
            
            Assert.True(result.HasInvalidRows);
        }

        [Test]
        public void ParserDoesNotSendAlertForValidRow()
        {
            var excelFile = CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()),
            });

            var result = parser.Parse(excelFile);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
            
            Assert.False(result.HasInvalidRows);
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

            var result = parser.Parse(excelFile, validateCells: true);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Once);

            Assert.True(result.HasInvalidCells);
            Assert.False(result.IsFileParsed);
        }

        [Test]
        public void ParserDoesNotSendAlert_For_RowsWithNoLowerThanThreeCells()
        {
            var excelFile = (CreateExcelFile(new List<Row>()
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
            }));

            mockExcelManager.Setup(x => x.Save(excelFile)).Returns(true);

            var result = parser.Parse(excelFile, validateCells: true);

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
            
            Assert.False(result.HasInvalidCells);
            Assert.True(result.IsFileParsed);
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

            var result = parser.Parse(excelFile, validateCells: true);

            Assert.True(result.IsFileParsed);
            Assert.True(result.WasSavedToStorage);
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

            var result = parser.Parse(excelFile, validateCells: true);

            Assert.False(result.IsFileParsed);
            Assert.False(result.WasSavedToStorage);
        }
    }
}