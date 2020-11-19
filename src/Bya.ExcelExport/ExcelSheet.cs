using System.Collections.Generic;
using System.Data;

namespace Bya.ExcelExport
{
    public class ExcelSheet
    {
        public ExcelSheet(DataTable table, string sheetName = "Sheet 1", uint sheetId = 1u)
        {
            SheetName = sheetName;
            SheetId = sheetId;
            Table = table;

            HeaderStyles = new List<ExcelStyleConfig>();
            RowsStyles = new List<ExcelStyleConfig>();
            ColumnsConfiguration = new List<ExcelColumnConfig>();
        }

        internal DataTable Table { get; set; }
        internal string SheetName { get; set; }
        internal uint SheetId { get; set; }

        public List<ExcelStyleConfig> HeaderStyles { get; set; }
        public List<ExcelStyleConfig> RowsStyles { get; set; }
        public List<ExcelColumnConfig> ColumnsConfiguration { get; set; }
    }
}
