namespace PdfGenerationWebServer {
    public interface IPdfGenerator {
        byte[] CreatePdfFromHtml(string html);
    }
}