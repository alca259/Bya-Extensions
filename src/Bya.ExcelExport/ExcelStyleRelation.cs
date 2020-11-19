using DocumentFormat.OpenXml;
using System.Collections.Generic;

namespace Bya.ExcelExport
{
    public class ExcelStyleRelation
    {
        #region Properties
        public string ApplyStyleOnSheetName { get; set; }
        public List<string> ApplyStyleOnColumns { get; set; }
        public UInt32Value StyleIndex { get; set; }
        public bool ApplyOnlyWhenValue { get; set; }
        #endregion

        #region Constructors
        public ExcelStyleRelation() { }
        public ExcelStyleRelation(string pApplySheet, List<string> pApplyColumns, UInt32Value pStyleIndex, bool pApplyWhenValue = false)
        {
            ApplyStyleOnSheetName = pApplySheet;
            ApplyStyleOnColumns = pApplyColumns;
            StyleIndex = pStyleIndex;
            ApplyOnlyWhenValue = pApplyWhenValue;
        }
        #endregion
    }
}
