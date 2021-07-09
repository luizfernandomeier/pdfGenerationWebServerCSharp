namespace PdfGenerationWebServer {
        public class IronPdfGenerator : IPdfGenerator {
        public byte[] CreatePdfFromHtml(string html) {
            var Renderer = new IronPdf.HtmlToPdf();
            var pdf = Renderer.RenderHtmlAsPdf(html);
            return pdf.BinaryData;
        }
    }
}