using Moq;
using nUnitTDD.Excel;

namespace nUnitTDD
{
    public class UnitTest1
    {
        public Mock<IAlertPublisher> mockAlertPublisher = new Mock<IAlertPublisher>();
        public Parser parser;

        private static ExcelFile CreateExcelFile(List<Row> rows)
        {
            return new ExcelFile(rows);
        }

        [SetUp]
        public void Setup()
        {
            mockAlertPublisher.Reset();
            parser = new Parser(mockAlertPublisher.Object);
        }

        [Test]
        public void ParserReturnsTwoAsParsedRowsCount()
        {
            var parsedCount = parser.Parse(CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>() { new Cell("1") }),
                new Row(new List <Cell>() { new Cell("1") }),
            }));

            Assert.AreEqual(2, parsedCount);
        }

        [Test]
        public void ParserReturnsThreeAsParsedRowsCount()
        {
            var parsedCount = parser.Parse(CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()),
                new Row(new List<Cell>()),
                new Row(new List<Cell>()),
            }));

            Assert.AreEqual(3, parsedCount);
        }

        [Test]
        public void ParserSendAlertForIneInvalidRow()
        {
            var parsedCount = parser.Parse(CreateExcelFile(new List<Row>()
            {
                new InvalidRow(),
            }));

            mockAlertPublisher.Verify(x => x.SendAlert());
        }

        [Test]
        public void ParserDoesNotSendAlertForIneInvalidRow()
        {
            var parsedCount = parser.Parse(CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()),
            }));

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
        }

        [Test]
        public void ParserSendAlert_For_RowWithLowerThanThreeCells()
        {
            var parsedCount = parser.ParseWithCells(CreateExcelFile(new List<Row>()
            {
                new Row(new List<Cell>()
                {
                    new Cell("1"),
                    new Cell("2"),
                }),
            }));

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Once);
        }

        [Test]
        public void ParserDoesNotSendAlert_For_RowsWithNoLowerThanThreeCells()
        {
            var parsedCount = parser.ParseWithCells(CreateExcelFile(new List<Row>()
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

            mockAlertPublisher.Verify(x => x.SendAlert(), Times.Never);
        }
    }
}