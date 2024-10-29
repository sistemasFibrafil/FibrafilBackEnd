﻿using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Net.CrossCotting
{
    public class PageEventHelper : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate template;

        PdfTemplate templateHeader;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private Font _HeaderFont;
        public Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private Font _FooterFont;
        public Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        private Boolean _FlagCerrado;
        public Boolean FlagCerrado
        {
            get { return _FlagCerrado; }
            set { _FlagCerrado = value; }
        }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(document.PageSize.Width, 80);
                templateHeader = cb.CreateTemplate(document.PageSize.Width, 80);
            }
            catch (DocumentException)
            {
            }
            catch (System.IO.IOException)
            {
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;
            if (Title != string.Empty)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 15);
                cb.SetRgbColorFill(50, 50, 200);
                cb.SetColorFill(new BaseColor(103, 93, 152));
                //cb.SetRgbColorFillF(103, 93, 152);

                //WriteWaterMark(document, "sss");
                cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
                cb.ShowText(Title);
                cb.EndText();
            }
            if (HeaderLeft + HeaderRight != string.Empty)
            {
                //PdfPTable HeaderTable = new PdfPTable(2);
                //HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //HeaderTable.TotalWidth = pageSize.Width;
                //HeaderTable.SetWidthPercentage(new float[] { 50, 50 }, pageSize);

                //PdfPCell HeaderLeftCell = new PdfPCell(new Phrase(8, HeaderLeft, HeaderFont)) { BorderColor = new BaseColor(54, 207, 37), BackgroundColor = new BaseColor(54, 207, 37) };
                //HeaderLeftCell.Padding = 5;
                //HeaderLeftCell.PaddingBottom = 8;
                //HeaderLeftCell.BorderWidthRight = 0;
                ////HeaderLeftCell.BackgroundColor = new BaseColor(103, 93, 152);
                //HeaderTable.AddCell(HeaderLeftCell);
                //PdfPCell HeaderRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont)) { BorderColor = new BaseColor(237, 77, 7), BackgroundColor = new BaseColor(237, 77, 7) };
                //HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //HeaderRightCell.Padding = 5;
                //HeaderRightCell.PaddingBottom = 8;
                //HeaderRightCell.BorderWidthLeft = 0;
                //HeaderTable.AddCell(HeaderRightCell);
                //cb.SetRgbColorFill(0, 0, 0);
                //HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(0), pageSize.GetTop(0), cb);
            }

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(Environment.CurrentDirectory, "img", "header.png"));
            //float w = document.PageSize.Width;
            //float h = 80;
            //logo.Alignment = Element.ALIGN_CENTER;
            //templateHeader.AddImage(logo, w, 0, 0, h, 0, 0);

            //cb.AddTemplate(templateHeader, pageSize.GetLeft(0), pageSize.GetTop(80));
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            //String text = "Page " + pageN + "/";
            //float len = bf.GetWidthPoint(text, 8);
            Rectangle pageSize = document.PageSize;
            cb.SetRgbColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
            //cb.ShowText(text);
            cb.EndText();

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(Environment.CurrentDirectory, "img", "footer.png"));
            //float w = document.PageSize.Width;
            //float h = 80;
            //logo.Alignment = Element.ALIGN_CENTER;
            //template.AddImage(logo, w, 0, 0, h, 0, 0);

            //cb.AddTemplate(template, pageSize.GetLeft(0), pageSize.GetBottom(0));
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 8);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
            //    "Printed On " + PrintTime.ToString(),
            //    pageSize.GetRight(40),
            //    pageSize.GetBottom(30), 0);
            //cb.EndText();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            //template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }

        private void WriteWaterMark(Document objPdfDocument, string strFileImage)
        {
            iTextSharp.text.Image objImagePdf;

            // Crea la imagen
            if (this.FlagCerrado)
            {
                objImagePdf = iTextSharp.text.Image.GetInstance(Path.Combine(Environment.CurrentDirectory, "img", "pollitos.png"));
            }
            else
            {
                objImagePdf = iTextSharp.text.Image.GetInstance(Path.Combine(Environment.CurrentDirectory, "img", "pollitos_borrador.png"));
            }

            // Cambia el tamaño de la imagen
            objImagePdf.ScaleToFit(objPdfDocument.PageSize.Width, objPdfDocument.PageSize.Height);
            // Se indica que la imagen debe almacenarse como fondo
            objImagePdf.Alignment = iTextSharp.text.Image.UNDERLYING;
            // Coloca la imagen en una posición absoluta
            objImagePdf.SetAbsolutePosition(0, 320);
            //objImagePdf.SetAbsolutePosition(7, 69);
            // Imprime la imagen como fondo de página
            objPdfDocument.Add(objImagePdf);
        }
    }
}
