using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Lexus2_0.Datos.Auxiliares
{
    public class ImageRenderListener : IEventListener
    {
        public ImageRenderListener()
        {
            //string format
            //this.format = format;
        }

        //public void EventOccurred(IEventData data, EventType type)
        //{
        //    if (data is ImageRenderInfo)
        //    {
        //        try
        //        {
        //            ImageRenderInfo imageData = (ImageRenderInfo)data;
        //            PdfImageXObject imageObject = imageData.GetImage();
        //            if (imageObject == null)
        //            {
        //                Console.WriteLine("Image could not be read.");
        //            }
        //            else
        //            {
        //                File.WriteAllBytes(string.Format(format, index++, imageObject.IdentifyImageFileExtension()), imageObject.GetImageBytes());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Image could not be read: {0}.", ex.Message);
        //        }
        //    }
        //}
        public void EventOccurred(IEventData data, EventType type)
        {
            if (data is ImageRenderInfo)
            {
                try
                {
                    ImageRenderInfo imageData = (ImageRenderInfo)data;
                    PdfImageXObject imageObject = imageData.GetImage();
                    if (imageObject == null)
                    {
                        Console.WriteLine("Image could not be read.");
                    }
                    else
                    {
                        //File.WriteAllBytes(string.Format(format, index++, imageObject.IdentifyImageFileExtension()), imageObject.GetImageBytes());
                        using (var ms = new MemoryStream(imageObject.GetImageBytes()))
                        {
                            Images.Add(new Bitmap(ms));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Image could not be read: {0}.", ex.Message);
                }
            }
        }
        public ICollection<EventType> GetSupportedEvents()
        {
            return null;
        }
        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
        //string format;
        //int index = 0;
    }
}