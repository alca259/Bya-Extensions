using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;

namespace Bya.ExcelExport
{
    public class ExcelStyleConfig
    {
        #region Properties
        /// <summary>
        /// Hoja de estilos a la que afecta
        /// </summary>
        public string SheetNameStyle { get; set; }
        /// <summary>
        /// Lista de columnas en los que se aplica este estilo
        /// </summary>
        public List<string> HeaderName { get; set; }
        /// <summary>
        /// This rule apply only when cell have value
        /// </summary>
        public bool ApplyOnlyWhenValue { get; set; }
        private Alignment AlignmentValue { get; set; }
        private Font FontValue { get; set; }
        private Fill FillValue { get; set; }
        private Border BorderValue { get; set; }
        private NumberingFormat NumberingFormatValue { get; set; }
        #endregion

        #region Constructors
        public ExcelStyleConfig()
        {
            SetFont();
            SetFill();
            ApplyOnlyWhenValue = false;
        }
        #endregion

        #region Getters & Setters
        public Alignment GetAlignment()
        {
            return AlignmentValue;
        }

        public void SetAlignment(HorizontalAlignmentValues hAlign, VerticalAlignmentValues vAlign, bool wrapText = true)
        {
            AlignmentValue = new Alignment { Horizontal = hAlign, Vertical = vAlign, WrapText = wrapText };
        }

        public Font GetFont()
        {
            return FontValue;
        }

        public void SetFont(string fontFamilyName = "Arial", double? fontSize = 10, bool isBold = false, bool isItalic = false, string fontHexForeColor = "00000000")
        {
            FontValue = CreateFont(fontHexForeColor, fontFamilyName, fontSize, isBold, isItalic);
        }

        public Fill GetFill()
        {
            return FillValue;
        }

        /// <summary>
        /// Establece el color de fondo
        /// </summary>
        /// <param name="backHexForeColor">Es el color de fondo, los dos primeros son el Alfa (dejar siempre a FF)</param>
        /// <param name="backHexBackColor">Es un color, pero ni idea</param>
        public void SetFill(string backHexForeColor = "FFFFFFFF", string backHexBackColor = "FFFFFFFF")
        {
            FillValue = CreateFill(backHexForeColor, backHexBackColor);
        }

        public Border GetBorder()
        {
            return BorderValue;
        }

        public void SetBorder(string borderColor = "00000000", BorderStyleValues borderSize = BorderStyleValues.Thin, bool leftBorder = true, bool rightBorder = true, bool topBorder = true, bool bottomBorder = true)
        {
            BorderValue = CreateBorder(leftBorder, rightBorder, topBorder, bottomBorder, borderColor, borderSize);
        }

        public NumberingFormat GetNumberingFormat()
        {
            return NumberingFormatValue;
        }

        public void SetNumberingFormat(int numberIndex = 4, string formatCode = "#,##0.00")
        {
            NumberingFormatValue = CreateNumberingFormat(numberIndex, formatCode);
        }

        #endregion

        #region Private Methods
        public static Font CreateFont(string hexForeColor, string fontName, double? fontSize, bool isBold, bool isItalic)
        {
            // Creamos una fuente nueva
            Font font = new Font();

            // Si tiene otra fuente diferente a la de por defecto, la cambiamos
            if (!string.IsNullOrEmpty(fontName))
            {
                font.Append(new FontName { Val = fontName });
            }

            // Si tiene un tamaño diferente al de por defecto, lo cambiamos
            if (fontSize.HasValue)
            {
                font.Append(new FontSize { Val = fontSize.Value });
            }

            // Si es negrita lo cambiamos
            if (isBold)
            {
                font.Append(new Bold());
            }

            // Si es cursiva, lo cambiamos
            if (isItalic)
            {
                font.Append(new Italic());
            }

            // Establecemos el color de la fuente
            Color color = new Color
            {
                Rgb = HexBinaryValue.FromString(hexForeColor)
            };

            font.Append(color);

            return font;
        }

        public static Fill CreateFill(string foreHexColor, string backHexColor)
        {
            PatternFill solidColor = new PatternFill { PatternType = PatternValues.Solid };

            solidColor.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString(foreHexColor) };
            solidColor.BackgroundColor = new BackgroundColor { Rgb = HexBinaryValue.FromString(backHexColor) };

            return new Fill { PatternFill = solidColor };
        }

        public static Border CreateBorder(bool leftBorder, bool rightBorder, bool topBorder, bool bottomBorder, string borderColor, BorderStyleValues borderSize)
        {
            Border border = new Border();
            Color color = new Color
            {
                Rgb = HexBinaryValue.FromString(borderColor)
            };

            if (leftBorder)
            {
                LeftBorder leftBorder2 = new LeftBorder() { Style = borderSize };
                leftBorder2.Color = color;
                border.LeftBorder = leftBorder2;
            }

            if (rightBorder)
            {
                RightBorder rightBorder2 = new RightBorder() { Style = borderSize };
                rightBorder2.Color = (Color)color.CloneNode(true);
                border.RightBorder = rightBorder2;
            }

            if (topBorder)
            {
                TopBorder topBorder2 = new TopBorder() { Style = borderSize };
                topBorder2.Color = (Color)color.CloneNode(true);
                border.TopBorder = topBorder2;
            }

            if (bottomBorder)
            {
                BottomBorder bottomBorder2 = new BottomBorder() { Style = borderSize };
                bottomBorder2.Color = (Color)color.CloneNode(true);
                border.BottomBorder = bottomBorder2;
            }

            return border;
        }

        public static NumberingFormat CreateNumberingFormat(int valueIdx, string formato)
        {
            NumberingFormat format = new NumberingFormat();

            if (valueIdx > 0)
            {
                format.NumberFormatId = Convert.ToUInt32(valueIdx);
                format.FormatCode = StringValue.FromString(formato);
            }
            return format;
        }

        #endregion
    }
}
