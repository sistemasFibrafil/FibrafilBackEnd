using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Net.CrossCotting
{
    public static class ExportToExcel
    {
        public static Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        public static Cell ConstructCellStyle(string value, CellValues dataType, uint styleIndex)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        public static Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet
                (
                    new Fonts
                    (
                        new Font(new FontSize() { Val = 12 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Calibri" }),//Titulos sin negrita
                        new Font(new Bold(), new FontSize() { Val = 12 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Calibri" }),//Titulos con negrita
                        new Font(new FontSize() { Val = 10.5 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Calibri" }),//Contenidos
                        new Font(new Bold(), new FontSize() { Val = 12 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Times New Roman" }),
                        new Font(new FontSize() { Val = 12 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Times New Roman" }),
                        new Font(new FontSize() { Val = 14 }, new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }, new FontName() { Val = "Times New Roman" })
                    ),
                    new Fills
                    (
                        new Fill(new PatternFill() { PatternType = PatternValues.None }),//FillId = 0
                        new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFAAAAAA" } }) { PatternType = PatternValues.Solid }),//FillId = 1
                        new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFC000" } }) { PatternType = PatternValues.Solid }),//FillId = 2
                        new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFAEAE" } }) { PatternType = PatternValues.Solid }) //FillId = 3
                    ),
                    new Borders
                    (
                        new Border
                        (
                            new LeftBorder(),
                            new RightBorder(),
                            new TopBorder(),
                            new BottomBorder(),
                            new DiagonalBorder()
                        ),
                        new Border
                        (
                            new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },
                            new RightBorder(new Color() { Indexed = (UInt32Value)64U }) { Style = BorderStyleValues.Medium },
                            new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },
                            new BottomBorder(new Color() { Indexed = (UInt32Value)64U }) { Style = BorderStyleValues.Medium },
                            new DiagonalBorder()
                        ),
                        new Border
                        (
                            new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                            new RightBorder(new Color() { Indexed = (UInt32Value)64U }) { Style = BorderStyleValues.Thin },
                            new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                            new BottomBorder(new Color() { Indexed = (UInt32Value)64U }) { Style = BorderStyleValues.Thin }, new DiagonalBorder()
                        )
                    ),
                    new CellFormats
                    (
                        new CellFormat() { FontId = 1, FillId = 0, BorderId = 0 },//StyleIndex = 0
                        new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 1, FillId = 2, BorderId = 1, ApplyFont = true },//StyleIndex = 1, Titulo
                        new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true },//StyleIndex = 2, SubTitulo
                        new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 0, FillId = 2, BorderId = 0, ApplyFont = true, ApplyFill = true },//StyleIndex = 3, Cabecera
                        new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },//StyleIndex = 4, Tipo texto
                        new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 0, FillId = 0, BorderId = 0, ApplyFont = true, NumberFormatId = 0 },//StyleIndex = 5, Tipo número
                        new CellFormat() { FontId = 3, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 },
                        new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true }
                    )
              );
        }
    }
}
