using ImageMagick;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GIFGen
{
    public static class GIFGen
    {
        //https://stackoverflow.com/questions/1196322/how-to-create-an-animated-gif-in-net
        public static MagickImageCollection GetGIFObject(List<BitmapSource> imageList, bool optimize, int animationDelayMS)
        {
            MagickImageCollection collection = new MagickImageCollection();
            for (int i = 0; i < imageList.Count; i++)
            {
                using (var image = new MagickImage(bitmapToBytes(imageList[i])))
                {
                    collection.Add(image.Clone());
                    collection.Last().AnimationDelay = animationDelayMS;
                }
            }
            if (optimize) collection.Optimize();
            return collection;
        }

        private static byte[] bitmapToBytes(BitmapSource bitmapsource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();
            encoder.Frames.Add(BitmapFrame.Create(bitmapsource));
            encoder.Save(memoryStream);
            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();
            //imageToBytes
            MemoryStream ms = null;
            TiffBitmapEncoder enc = null;
            enc = new TiffBitmapEncoder();
            enc.Compression = TiffCompressOption.None;
            enc.Frames.Add(BitmapFrame.Create(bImg));
            using (ms = new MemoryStream())
            {
                enc.Save(ms);
            }
            memoryStream.Close();
            return ms.ToArray();
        }

    
    }

}
