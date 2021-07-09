using System;
using System.IO;
using Aspose.Pdf;

// NOT WORKING
namespace PdfGenerationWebServer {
    public class AsposePdfGenerator : IPdfGenerator {
        public byte[] CreatePdfFromHtml(string html) {

            var filename = Guid.NewGuid().ToString();
            File.WriteAllText($"/tmp/pdf/{filename}.html", html);

            var options = new HtmlLoadOptions();
            var pdfDocument= new Document($"/tmp/pdf/{filename}.html", options);
            pdfDocument.Save($"/tmp/pdf/{filename}.pdf");

            return File.ReadAllBytes($"/tmp/pdf/{filename}.pdf");
        }
    }
}