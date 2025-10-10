namespace nUnitTDD.Excel.FileStructure
{
    public class Cell
    {
        public string Value { get; set; }

        public Cell(string value = "")
        {
            Value = value;
        }
    }
}
