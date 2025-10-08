namespace nUnitTDD.Excel
{
    public class Row
    {
        public List<Cell> Cells { get; set; }

        public Row(List<Cell> Cells)
        {
            this.Cells = Cells;
        }
    }
}
