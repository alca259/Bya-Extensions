using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Bya.ExcelExport
{
    public static class ExcelExporter
    {
        #region Public methods
        public static MemoryStream ExportToMemoryStream<T>(
            IEnumerable<T> data,
            string sheetName = "Sheet 1",
            uint sheetId = 1u,
            List<ExcelColumnConfig> columnConfig = null,
            List<ExcelStyleConfig> headerStyles = null,
            List<ExcelStyleConfig> rowStyles = null) where T : class
        {
            var table = ConvertToDatatable(data);
            var excelSheet = new ExcelSheet(table, sheetName, sheetId);

            if (columnConfig != null)
            {
                excelSheet.ColumnsConfiguration = columnConfig;
            }

            if (headerStyles != null)
            {
                excelSheet.HeaderStyles = headerStyles;
            }

            if (rowStyles != null)
            {
                excelSheet.RowsStyles = rowStyles;
            }

            var stream = new MemoryStream();
            ExportAdvancedMode(stream, new List<ExcelSheet> { excelSheet });
            return stream;
        }

        public static void ExportToStream<T>(
            Stream stream,
            IEnumerable<T> data,
            string sheetName = "Sheet 1",
            uint sheetId = 1u,
            List<ExcelColumnConfig> columnConfig = null,
            List<ExcelStyleConfig> headerStyles = null,
            List<ExcelStyleConfig> rowStyles = null) where T : class
        {
            var table = ConvertToDatatable(data);
            var excelSheet = new ExcelSheet(table, sheetName, sheetId);

            if (columnConfig != null)
            {
                excelSheet.ColumnsConfiguration = columnConfig;
            }

            if (headerStyles != null)
            {
                excelSheet.HeaderStyles = headerStyles;
            }

            if (rowStyles != null)
            {
                excelSheet.RowsStyles = rowStyles;
            }

            ExportAdvancedMode(stream, new List<ExcelSheet> { excelSheet });
        }

        public static void ExportAdvancedMode(Stream stream, List<ExcelSheet> excelSheets)
        {
            // La clase SpreadsheetDocument representa un paquete de documentos de Excel.
            // Para crear un documento de Excel, cree una instancia de la clase SpreadsheetDocument y rellénela con partes.
            // Como mínimo, el documento debe tener una parte de libro que sirva como contenedor para el documento y una parte de hoja de cálculo.
            // Por defecto AutoSave = true, Editable = true, y Type = xlsx.
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                // Añadimos un elemenro de libro al documento (es obligatorio)
                var wbp = document.AddWorkbookPart();
                wbp.Workbook = new Workbook();

                // Añadimos hojas al libro
                Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                foreach (var excelSheet in excelSheets)
                {
                    var sheetName = excelSheet.SheetName.ToLower();

                    // START
                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                    Worksheet ws = new Worksheet();
                    SheetData sd = new SheetData();
                    // END START

                    #region DATA

                    #region Style configuration
                    // Intentamos obtener el estilo
                    WorkbookStylesPart styles = wbp.WorkbookStylesPart;

                    #region Estilos por defecto
                    if (styles == null)
                    {
                        // Creamos un nuevo estilo
                        styles = wbp.AddNewPart<WorkbookStylesPart>();
                        styles.Stylesheet = new Stylesheet();

                        // Build the formatted header style
                        styles.Stylesheet.Fonts = new Fonts { Count = 1 };
                        styles.Stylesheet.Fonts.AppendChild(new Font()); // required, reserved by Excel

                        styles.Stylesheet.Fills = new Fills { Count = 2 };
                        styles.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
                        styles.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel

                        styles.Stylesheet.Borders = new Borders { Count = 1 };
                        styles.Stylesheet.Borders.AppendChild(new Border()); // required, reserved by Excel

                        styles.Stylesheet.CellStyleFormats = new CellStyleFormats { Count = 1 };
                        styles.Stylesheet.CellStyleFormats.AppendChild(new CellFormat()); // required, reserved by Excel

                        styles.Stylesheet.NumberingFormats = new NumberingFormats { Count = 0 };
                        //styles.Stylesheet.NumberingFormats.AppendChild(new NumberingFormat {NumberFormatId = 0, FormatCode = "."}); // required, reserved by Excel

                        styles.Stylesheet.CellFormats = new CellFormats { Count = 1 };
                        styles.Stylesheet.CellFormats.AppendChild(new CellFormat()); // required, reserved by Excel
                    }
                    #endregion

                    #region Estilo de cabecera
                    List<ExcelStyleRelation> styleHeaderRelation = new List<ExcelStyleRelation>();
                    if (excelSheet.HeaderStyles.Any())
                    {
                        foreach (ExcelStyleConfig styleConfig in excelSheet.HeaderStyles.Where(w => w.SheetNameStyle.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // Convertimos todas las lineas a minusculas
                            styleConfig.SheetNameStyle = styleConfig.SheetNameStyle.ToLower();
                            styleConfig.HeaderName = styleConfig.HeaderName.ConvertAll(d => d.ToLower());

                            UInt32Value headerFontIndex = 0;
                            UInt32Value headerFillIndex = 0;
                            UInt32Value headerBorderIndex = 0;
                            UInt32Value headerStyleFormatIndex = 0;
                            UInt32Value headerStyleIndex = 0;

                            if (styleConfig.GetFont() != null)
                                headerFontIndex = CreateFont(styles.Stylesheet, styleConfig.GetFont());

                            if (styleConfig.GetFill() != null)
                                headerFillIndex = CreateFill(styles.Stylesheet, styleConfig.GetFill());

                            if (styleConfig.GetBorder() != null)
                                headerBorderIndex = CreateBorder(styles.Stylesheet, styleConfig.GetBorder());

                            headerStyleIndex = CreateCellFormat(styles.Stylesheet, headerStyleFormatIndex, headerFontIndex, headerBorderIndex, headerFillIndex, null, styleConfig.GetAlignment());

                            styleHeaderRelation.Add(new ExcelStyleRelation(styleConfig.SheetNameStyle, styleConfig.HeaderName, headerStyleIndex));
                        }
                    }
                    #endregion

                    #region Estilo de filas
                    var styleRowsRelation = new List<ExcelStyleRelation>();
                    if (excelSheet.RowsStyles.Any())
                    {
                        foreach (var styleConfig in excelSheet.RowsStyles.Where(w => w.SheetNameStyle.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // Convertimos todas las lineas a minusculas
                            styleConfig.SheetNameStyle = styleConfig.SheetNameStyle.ToLower();
                            styleConfig.HeaderName = styleConfig.HeaderName.ConvertAll(d => d.ToLower());

                            UInt32Value rowFontIndex = 0;
                            UInt32Value rowFillIndex = 0;
                            UInt32Value rowBorderIndex = 0;
                            UInt32Value rowNumberFormatIndex = 0;
                            UInt32Value rowStyleFormatIndex = 0;
                            UInt32Value rowStyleIndex = 0;

                            if (styleConfig.GetFont() != null)
                                rowFontIndex = CreateFont(styles.Stylesheet, styleConfig.GetFont());

                            if (styleConfig.GetFill() != null)
                                rowFillIndex = CreateFill(styles.Stylesheet, styleConfig.GetFill());

                            if (styleConfig.GetBorder() != null)
                                rowBorderIndex = CreateBorder(styles.Stylesheet, styleConfig.GetBorder());

                            if (styleConfig.GetNumberingFormat() != null)
                                rowNumberFormatIndex = CreateNumberFormat(styles.Stylesheet, styleConfig.GetNumberingFormat());

                            rowStyleIndex = CreateCellFormat(styles.Stylesheet, rowStyleFormatIndex, rowFontIndex, rowBorderIndex, rowFillIndex, rowNumberFormatIndex, styleConfig.GetAlignment());

                            styleRowsRelation.Add(new ExcelStyleRelation(styleConfig.SheetNameStyle, styleConfig.HeaderName, rowStyleIndex, styleConfig.ApplyOnlyWhenValue));
                        }
                    }
                    #endregion

                    // Save styles
                    styles.Stylesheet.Save();
                    #endregion

                    #region Header data
                    // Generamos la línea de cabecera
                    var headerRow = new Row
                    {
                        RowIndex = (uint)1
                    };

                    int columnIndex = 1;
                    foreach (DataColumn column in excelSheet.Table.Columns)
                    {
                        // Comprobamos si vienen datos en la configuración de las columnas
                        var dataColumn = excelSheet.ColumnsConfiguration.FirstOrDefault(f => column.ColumnName.Equals(f.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                        if (dataColumn == null)
                        {
                            dataColumn = new ExcelColumnConfig(column.ColumnName);
                            excelSheet.ColumnsConfiguration.Add(dataColumn);
                        }

                        // Si es visible, añadimos, si no, nos la saltamos, tanto aquí, como en las filas
                        if (dataColumn.ColumnVisible)
                        {
                            var cellHeader = new Cell
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(dataColumn.ColumnDisplayName),
                                CellReference = GetColumnName(columnIndex) + 1,
                            };

                            #region Aplicación de estilos de cabecera
                            var headerStyleFound = styleHeaderRelation
                                .Where(s => s.ApplyStyleOnSheetName.Equals(sheetName) && (s.ApplyStyleOnColumns.Contains(column.ColumnName.ToLower()) || s.ApplyStyleOnColumns.Contains("*")))
                                .ToList();

                            if (headerStyleFound.Any())
                            {
                                if (headerStyleFound.Count == 1)
                                {
                                    cellHeader.StyleIndex = headerStyleFound.Single().StyleIndex;
                                }
                                else
                                {
                                    var relation = headerStyleFound.First(w => !w.ApplyStyleOnColumns.Contains("*"));
                                    cellHeader.StyleIndex = relation.StyleIndex;
                                }
                            }
                            #endregion

                            headerRow.AppendChild(cellHeader);
                            columnIndex++;
                        }
                    }

                    // Añadimos la línea de cabecera a la hoja
                    sd.AppendChild(headerRow);
                    #endregion

                    #region Rows data
                    // Generamos cada una de las filas
                    foreach (DataRow row in excelSheet.Table.Rows)
                    {
                        // Creamos una nueva fila
                        var newRow = new Row();

                        // Para cada columna, vamos rellenando los datos
                        foreach (DataColumn col in excelSheet.Table.Columns)
                        {
                            // Comprobamos si vienen datos en la configuración de las columnas
                            var dataColumn = excelSheet.ColumnsConfiguration.First(f => col.ColumnName.ToLower().Equals(f.ColumnName.ToLower()));

                            // Si es visible, añadimos, si no, nos la saltamos, tanto aquí, como en las filas
                            if (!dataColumn.ColumnVisible) continue;

                            var cell = GetCellValues(row, col, dataColumn.ColumnDateFormat);

                            var haveValue = !string.IsNullOrEmpty(cell.CellValue.Text);

                            // Buscamos todos los estilos que apliquen a esta hoja de excel, y ademas apliquen a esta columna o a todas
                            var dataStyleFound = styleRowsRelation
                                .Where(s => s.ApplyStyleOnSheetName.Equals(sheetName) && (s.ApplyStyleOnColumns.Contains(col.ColumnName.ToLower()) || s.ApplyStyleOnColumns.Contains("*")))
                                .ToList();

                            if (!dataStyleFound.Any())
                            {
                                newRow.AppendChild(cell);
                                continue;
                            }

                            #region Aplicación de estilos de filas
                            if (dataStyleFound.Count == 1)
                            {
                                var relation = dataStyleFound.Single();

                                if (haveValue && relation.ApplyOnlyWhenValue)
                                {
                                    // Si este estilo aplica solamente cuando tiene valor y la celda tiene valor...
                                    cell.StyleIndex = relation.StyleIndex;
                                }
                                else if (!relation.ApplyOnlyWhenValue)
                                {
                                    // Si este estilo no aplica solamente cuando tiene valor, se lo asignamos
                                    cell.StyleIndex = relation.StyleIndex;
                                }
                            }
                            else
                            {
                                // Tiene varios estilos aplicando a esta columna
                                // La prioridad va en este orden:
                                // 1 - Si aplica a la columna y solo cuando tenga valor
                                // 2 - Si aplica a la columna
                                // 3 - Si aplica a todas las columnas

                                var relation = dataStyleFound.FirstOrDefault(f => f.ApplyOnlyWhenValue && !f.ApplyStyleOnColumns.Contains("*"));
                                if (relation != null && haveValue)
                                {
                                    cell.StyleIndex = relation.StyleIndex;
                                }
                                else
                                {
                                    relation = dataStyleFound.FirstOrDefault(f => !f.ApplyStyleOnColumns.Contains("*"));
                                    if (relation != null)
                                    {
                                        cell.StyleIndex = relation.StyleIndex;
                                    }
                                    else
                                    {
                                        relation = dataStyleFound.First();
                                        cell.StyleIndex = relation.StyleIndex;
                                    }
                                }
                            }
                            #endregion

                            newRow.AppendChild(cell);
                        }

                        // Añadimos la fila a la hoja
                        sd.AppendChild(newRow);
                    }

                    #endregion

                    #endregion

                    // END
                    ws.Append(sd);
                    wsp.Worksheet = ws;
                    wsp.Worksheet.Save();

                    Sheet sheet = new Sheet 
                    {
                        Name = excelSheet.SheetName,
                        SheetId = (UInt32Value)excelSheet.SheetId,
                        Id = wbp.GetIdOfPart(wsp)
                    };

                    sheets.Append(sheet);
                    // END END
                }

                // Guardamos el libro
                wbp.Workbook.Save();

                // Cerramos el documento
                document.Close();
            }

            stream.Seek(0, SeekOrigin.Begin);
        }

        public static DataTable ConvertToDatatable<T>(IEnumerable<T> data) where T : class
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            int countProperties = 0;

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.Name.Equals(typeof(Enum).Name))
                {
                    table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, typeof(string));
                }
                else if (prop.PropertyType.Name.Contains("Nullable"))
                {
                    if (prop.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, prop.PropertyType.GenericTypeArguments[0]);
                    }
                    else
                    {
                        table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, typeof(string));
                    }
                }
                else
                {
                    table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, prop.PropertyType);
                }
            }

            countProperties += props.Count;

            object[] values = new object[countProperties];
            foreach (T item in data)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    if (prop.PropertyType.Name.Equals(typeof(Enum).Name))
                    {
                        values[i] = ((Enum)props[i].GetValue(item)).ToString();
                    }
                    else
                    {
                        values[i] = props[i].GetValue(item);
                    }
                }
                table.Rows.Add(values);
            }
            return table;
        }
        #endregion

        #region Excel Helpers
        private static Cell GetCellValues(DataRow row, DataColumn column, string dateFormat = "dd/MM/yyyy")
        {
            var cell = new Cell();
            CellValue cellValue;
            var cellType = CellValues.String;

            var value = row[column, DataRowVersion.Current];
            var valueText = value.ToString();
            var valueType = value.GetType();


            // Ref: https://exceptionnotfound.net/decimal-vs-double-and-other-tips-about-number-types-in-net/
            switch (valueType.FullName)
            {
                case "System.Decimal":
                    cellType = CellValues.Number;
                    if (!string.IsNullOrEmpty(valueText))
                    {
                        cellValue = new CellValue(Math.Round(decimal.Parse(valueText), 28).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cellValue = new CellValue("0");
                    }
                    break;
                case "System.Double":
                    cellType = CellValues.Number;
                    if (!string.IsNullOrEmpty(valueText))
                    {
                        cellValue = new CellValue(Math.Round(double.Parse(valueText), 14).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cellValue = new CellValue("0");
                    }
                    break;
                case "System.Single":
                    cellType = CellValues.Number;
                    if (!string.IsNullOrEmpty(valueText))
                    {
                        cellValue = new CellValue(Math.Round(float.Parse(valueText), 7).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cellValue = new CellValue("0");
                    }
                    break;
                case "System.Int32":
                case "System.Int64":
                case "System.UInt32":
                case "System.UInt64":
                    cellType = CellValues.Number;
                    cellValue = new CellValue(valueText);
                    break;
                case "System.Date":
                case "System.DateTime":
                    //cellType = CellValues.Date; // NO USAR - POCO SOPORTADO Y CORROMPE FICHEROS
                    if (!string.IsNullOrEmpty(valueText))
                    {
                        DateTime dtValue = Convert.ToDateTime(valueText);
                        string strValue = dtValue.ToString(dateFormat);
                        cellValue = new CellValue(strValue);
                    }
                    else
                    {
                        string strValue = DateTime.Now.ToString(dateFormat);
                        cellValue = new CellValue(strValue);
                    }
                    break;
                case "System.Boolean":
                    cellType = CellValues.Boolean;
                    cellValue = new CellValue(valueText);
                    break;
                case "System.Byte[]":
                    if (!DBNull.Value.Equals(value))
                    {
                        byte[] dataBytes = (byte[])value;
                        cellValue = new CellValue(new Guid(dataBytes).ToString());
                    }
                    else
                    {
                        cellValue = new CellValue(string.Empty);
                    }
                    break;
                default:
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(valueText);
                    string s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                    cellValue = new CellValue(s_unicode2);
                    break;
            }

            cell.DataType = cellType;
            cell.CellValue = cellValue;

            return cell;
        }

        private static UInt32Value CreateFont(Stylesheet styleSheet, Font font)
        {
            // Agregamos la fuente al estilo y devolvemos resultado
            styleSheet.Fonts.Append(font);
            UInt32Value result = styleSheet.Fonts.Count;
            styleSheet.Fonts.Count++;
            return result;
        }

        private static UInt32Value CreateFill(Stylesheet styleSheet, Fill fill)
        {
            styleSheet.Fills.Append(fill);

            UInt32Value result = styleSheet.Fills.Count;
            styleSheet.Fills.Count++;
            return result;
        }

        private static UInt32Value CreateBorder(Stylesheet styleSheet, Border border)
        {
            styleSheet.Borders.Append(border);

            UInt32Value result = styleSheet.Borders.Count;
            styleSheet.Borders.Count++;
            return result;
        }

        private static UInt32Value CreateNumberFormat(Stylesheet styleSheet, NumberingFormat format)
        {
            styleSheet.NumberingFormats.Append(format);

            UInt32Value result = format.NumberFormatId;
            styleSheet.NumberingFormats.Count++;
            return result;
        }

        private static UInt32Value CreateCellFormat(Stylesheet styleSheet, UInt32Value formatIndex, UInt32Value fontIndex, UInt32Value borderIndex, UInt32Value fillIndex, UInt32Value numberFormatId, Alignment alignment)
        {
            CellFormat cellFormat = new CellFormat();

            if (fontIndex != null)
            {
                cellFormat.FontId = fontIndex;
                cellFormat.ApplyFont = true;
            }

            if (fillIndex != null)
            {
                cellFormat.FillId = fillIndex;
                cellFormat.ApplyFill = true;
            }

            if (numberFormatId != null && numberFormatId > 0)
            {
                cellFormat.NumberFormatId = numberFormatId;
                cellFormat.ApplyNumberFormat = true;
            }

            if (borderIndex != null)
            {
                cellFormat.BorderId = borderIndex;
                cellFormat.ApplyBorder = true;
            }

            if (alignment != null)
            {
                cellFormat.Alignment = alignment;
            }

            cellFormat.FormatId = formatIndex;

            styleSheet.CellFormats.Append(cellFormat);

            UInt32Value result = styleSheet.CellFormats.Count;
            styleSheet.CellFormats.Count++;
            return result;
        }

        private static string GetColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier) + columnName;
                dividend = ((dividend - modifier) / 26);
            }

            return columnName;
        }
        #endregion
    }
}
