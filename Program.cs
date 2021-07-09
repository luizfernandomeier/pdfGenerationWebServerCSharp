using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;

namespace PdfGenerationWebServer
{
    public class Program
    {
        const int PORT = 3020;

        static void Main(string[] args)
        {
            IPdfGenerator pdfGenerator = new IronPdfGenerator();
            //IPdfGenerator pdfGenerator = new SelectPdfGenerator();
            //IPdfGenerator pdfGenerator = new AsposePdfGenerator();

            var shouldExit = false;
            using (var shouldExitWaitHandle = new ManualResetEvent(shouldExit))
            using (var listener = new HttpListener())
            {
                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => {
                    e.Cancel = true;
                    shouldExit = true;
                    shouldExitWaitHandle.Set();
                };
                
                listener.Prefixes.Add($"http://*:{PORT}/");
                listener.Start();
                Console.WriteLine($"PDF server listening at port {PORT}");

                while (!shouldExit)
                {
                    var contextAsyncResult = listener.BeginGetContext((IAsyncResult asyncResult) =>
                        {
                            Console.WriteLine($"-");

                            var context = listener.EndGetContext(asyncResult);

                            string reqBody;
                            using (var reader = new StreamReader(context.Request.InputStream))
                                reqBody = reader.ReadToEnd();

                            var html = Regex.Replace(reqBody, "[-][-][a-z0-9]{60}[-][-]", "");
                            html = Regex.Replace(html, "[-][-][a-z0-9]{60}", "");
                            html = Regex.Replace(html, "Content-Disposition.+", "");
                            html = Regex.Replace(html, "Content-Type.+", "");

                            var pdf = pdfGenerator.CreatePdfFromHtml(html);

                            context.Response.ContentType = "application/pdf";
                            using (var writer = new BinaryWriter(context.Response.OutputStream))
                                writer.Write(pdf);
                        },
                        null
                    );

                    WaitHandle.WaitAny(new WaitHandle[]{
                        contextAsyncResult.AsyncWaitHandle,
                        shouldExitWaitHandle
                    });
                }

                listener.Stop();
            }
        }
    }
}
