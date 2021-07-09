namespace PdfGenerationWebServer {
    public class IronPdfGenerator : IPdfGenerator {
        public byte[] CreatePdfFromHtml(string html) {
            var renderer = new IronPdf.HtmlToPdf();
            var pdf = renderer.RenderHtmlAsPdf(html);
            return pdf.BinaryData;
        }
    }
}
