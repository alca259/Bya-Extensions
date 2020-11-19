namespace Bya.ExcelExport
{
    public class ExcelColumnConfig
    {
        #region Fields
        // Name field
        private string _ColumnDisplayName;
        #endregion

        #region Properties
        /// <summary>
        /// Nombre de la columna de oracle
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Nombre de la columna a mostrar, por defecto el nombre de la columna de oracle
        /// </summary>
        public string ColumnDisplayName
        {
            get { return string.IsNullOrEmpty(_ColumnDisplayName) ? ColumnName : _ColumnDisplayName; }
            set { _ColumnDisplayName = value; }
        }

        /// <summary>
        /// Indica si la columna es visible o no
        /// </summary>
        public bool ColumnVisible { get; set; }

        /// <summary>
        /// Formato de fecha
        /// </summary>
        public string ColumnDateFormat { get; set; }

        /// <summary>
        /// Indica si la columna se tiene que interpretar como una formula o no
        /// </summary>
        public bool ColumnFormulaData { get; set; }
        #endregion

        #region Constructors
        public ExcelColumnConfig()
        {
            ColumnVisible = true;
            ColumnDateFormat = "dd/MM/yyyy hh:mm:ss";
        }

        public ExcelColumnConfig(string pColumnName, bool pVisible = true)
        {
            ColumnName = pColumnName;
            ColumnVisible = pVisible;
            ColumnDateFormat = "dd/MM/yyyy hh:mm:ss";
        }

        public ExcelColumnConfig(string pColumnName, string pDisplayName, bool pVisible = true, string pColumnDateFormat = null, bool pColumnFormulaData = false)
        {
            ColumnName = pColumnName;
            ColumnDisplayName = pDisplayName;
            ColumnVisible = pVisible;
            ColumnDateFormat = string.IsNullOrEmpty(pColumnDateFormat) ? "dd/MM/yyyy hh:mm:ss" : pColumnDateFormat;
            ColumnFormulaData = pColumnFormulaData;
        }
        #endregion
    }
}
