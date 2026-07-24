using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminDocumentos
    {
        //public static string ObtenerTextoDeArchivo(HttpPostedFile archivo)
        //{
        //    string texto = "";
        //    try
        //    {
        //        string nombre = archivo.FileName;
        //        string tipo = archivo.ContentType;
        //        if (tipo.Contains("pdf"))
        //        {
        //            texto = ObtenerTextoDePDF(archivo);
        //        }
        //        else if (tipo.Contains("image"))
        //        {
        //            Bitmap bitMap = new Bitmap(archivo.InputStream);
        //            TesseractEngine engine = new TesseractEngine(Server.MapPath("~/tessdata"), "spa", EngineMode.Default);
        //            using (var tPage = engine.Process(bitMap))
        //            {
        //                texto += $"| {tPage.GetText()}";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return texto;
        //}

        //private static string ObtenerTextoDePDF(HttpPostedFile archivo)
        //{
        //    return "";
        //    ////    BinaryReader rdr = new BinaryReader(archivo.InputStream);
        //    ////    byte[] pdfbytes = rdr.ReadBytes(archivo.ContentLength);

        //    //PdfReader Reader = new PdfReader(archivo.InputStream);
        //    ////PdfReader Reader = new PdfReader(Server.MapPath("~/Pruebas/iTextSharp/handDocumentos.pdf"));
        //    //var pdfDocument = new PdfDocument(Reader);
        //    //var strategy = new LocationTextExtractionStrategy();
        //    //StringBuilder processed = new StringBuilder();
        //    //for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
        //    //{
        //    //    var page = pdfDocument.GetPage(i);
        //    //    string text = PdfTextExtractor.GetTextFromPage(page);//, strategy);
        //    //    processed.Append(text);
        //    //}
        //    //string txtPDF = processed.ToString();
        //    //if (string.IsNullOrEmpty(txtPDF))
        //    //{






        //    //    byte[] bytesArchivoPDF = null;
        //    //    archivo.InputStream.Position = 0;
        //    //    using (var binaryReader = new BinaryReader(archivo.InputStream))
        //    //    {
        //    //        bytesArchivoPDF = new byte[(Int32)archivo.ContentLength];
        //    //        bytesArchivoPDF = binaryReader.ReadBytes((Int32)archivo.ContentLength);
        //    //        binaryReader.Close();
        //    //    }
        //    //    //BUSCAR SI EL PDF TIENE TEXTO ESCRITO (QUE NO SEA DE IMAGEN)
        //    //    //var pdfDocumentOld = new iTextSharp.text.pdf.PdfDocument(new iTextSharp.text.pdf.PdfReader(bytesArchivoPDF));
        //    //    //PDFParser parser = new PDFParser();
        //    //    //txtPDF = parser.ExtractText(bytesArchivo);
        //    //    if (string.IsNullOrEmpty(txtPDF))
        //    //    {
        //    //        //SE TRATA DE UN PDF CON IMÁGENES, CONVERTIR EL PDF EN IMÁGENES...
        //    //        //Dictionary<string, System.Drawing.Image> Paginas = PdfImageExtractor.ExtractImages(bytesArchivoPDF, archivo.FileName);

        //    //        var images = new Dictionary<string, System.Drawing.Image>();
        //    //        using (Reader)
        //    //        {
        //    //            //var parser = new   PdfReaderContentParser(reader);
        //    //            //ImageRenderListener listener = null;
        //    //            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
        //    //            {
        //    //                PdfDictionary obj = (PdfDictionary)pdfDocument.GetPdfObject(i);
        //    //                if (obj != null)// && obj.IsStream())
        //    //                {
        //    //                    PdfDictionary pd = (PdfDictionary)obj;
        //    //                    //if (pd.ContainsKey(PdfName.Subtype) && pd.Get(PdfName.Subtype).ToString() == "/Image")
        //    //                    {
        //    //                        string filter = pd.Get(PdfName.Filter).ToString();
        //    //                        string width = pd.Get(PdfName.Width).ToString();
        //    //                        string height = pd.Get(PdfName.Height).ToString();
        //    //                        string bpp = pd.Get(PdfName.BitsPerComponent).ToString();
        //    //                        string extent = ".";
        //    //                        byte[] img = null;
        //    //                        switch (filter)
        //    //                        {
        //    //                            case "/FlateDecode":
        //    //                                byte[] arr = FlateDecodeFilter.FlateDecode(null, true);
        //    //                                Bitmap bmp = new Bitmap(Int32.Parse(width), Int32.Parse(height), PixelFormat.Format24bppRgb);
        //    //                                BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, Int32.Parse(width), Int32.Parse(height)), ImageLockMode.WriteOnly,
        //    //                                    PixelFormat.Format24bppRgb);
        //    //                                Marshal.Copy(arr, 0, bmd.Scan0, arr.Length);
        //    //                                bmp.UnlockBits(bmd);
        //    //                                TesseractEngine engine = new TesseractEngine(Server.MapPath("~/tessdata"), "spa", EngineMode.Default);
        //    //                                using (var tPage = engine.Process(bmp))
        //    //                                {
        //    //                                    txtPDF = tPage.GetText();
        //    //                                }
        //    //                                //bmp.Save("d:\\pdf\\bmp1.png", System.Drawing.Imaging.ImageFormat.Png);
        //    //                                break;
        //    //                            case "/CCITTFaxDecode":
        //    //                                break;
        //    //                            default:
        //    //                                break;
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }
        //    //        }

        //    //        //LUEGO USAR EL OCL TESSERACT PARA LEER EL TEXTO

        //    //    }

        //    //}
        //    //return txtPDF;
        //}
    }
}