namespace PdfGenerationWebServer {
        public class SelectPdfGenerator : IPdfGenerator {
        public byte[] CreatePdfFromHtml(string html) {
            var renderer = new SelectPdf.HtmlToPdf();
            var pdf = renderer.ConvertHtmlString(html);
            return pdf.Save();
        }
    }
}