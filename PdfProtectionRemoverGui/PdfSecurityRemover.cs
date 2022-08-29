using System;
using System.Windows;

using iText.Kernel.Pdf;

namespace PdfProtectionRemoverGui
{
    internal static class PdfSecurityRemover
    {
        internal static void SecurityRemove(string inputFileName, string outputFileName)
        {
            try
            {
                using var pdfReader = new PdfReader(inputFileName);
                //Без этого метода ничего не работает
                pdfReader.SetUnethicalReading(true);
                using var protectedPdfDocument = new PdfDocument(pdfReader);

                using var pdfWriter = new PdfWriter(outputFileName);
                using var unprotectedPdfDocument = new PdfDocument(pdfWriter);

                protectedPdfDocument.CopyPagesTo(1, protectedPdfDocument.GetNumberOfPages(), unprotectedPdfDocument);

                protectedPdfDocument.Close();
                unprotectedPdfDocument.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при снятии защиты: {e.Message}");
            }
        }
    }
}
