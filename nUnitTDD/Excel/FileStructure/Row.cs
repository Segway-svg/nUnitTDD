namespace nUnitTDD.Excel.FileStructure
{
    public class Row
    {
        public bool IsValid { get; set; }   
        public List<Cell> Cells { get; set; }

        public Row() { }

        public Row(List<Cell> Cells)
        {
            this.Cells = Cells;
        }
    }
}