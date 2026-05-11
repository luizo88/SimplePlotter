using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GifTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, double>> data = new List<Tuple<string, double>>();
            //test1a
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test1\5000-10-15w\";
            //List<double> id = new List<double> { 0, 1156, 1227, 1297, 1384, 2737, 2986, 3007, 3122, 3140, 3152, 3174, 3176, 3195, 3314, 3330, 3430, 3493, 3576, 3624, 3632, 3668, 3677, 3719, 3731, 3764, 3967, 4027, 5000 };
            //test1b
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test1\5000-100-15w\";
            //List<int> id = new List<int> { 0, 19, 51, 54, 72, 83, 85, 87, 95, 101, 104, 108, 110, 114, 117, 118, 119, 120, 122, 127, 129, 133, 164, 187, 204, 243, 255, 778, 1325, 5000 };
            //test2a
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test2\5000-10-7w\";
            //List<int> id = new List<int> { 0, 7, 9, 10, 11, 12, 13, 15, 16, 17, 18, 20, 21, 22, 23, 25, 26, 27, 28, 29, 30, 31, 33, 34, 35, 37, 38, 39, 40, 41, 42, 43, 52, 59, 66, 105, 114, 115 };
            //test2b
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test2\5000-100-7w\";
            //List<int> id = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 29, 32, 38, 107 };
            //test2a
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test3\5000-10-7\";
            //List<int> id = new List<int> { 0, 1, 12, 18, 55, 67, 100 };
            //test2b
            //string path = @"E:\OneDrive\Assuntos técnicos\Artigos\LinkedIn\08 - Otimização do núcleo\Gifs\Test3\5000-100-7\";
            //List<int> id = new List<int> { 0, 1, 2, 5, 9, 17, 70, 100 };
            //phase shift gif
            string path = @"C:\Users\Admin\TRINSE\TRINSE - Documentos\02.Divulgação e Mkt\05.LinkedIn\04.CFD in secos\";
            List<int> id = new List<int>();
            for (int i = 1; i <= 5; i++)
            {
                id.Add(i);
            }
            for (int i = 0; i < id.Count; i++)
            {
                data.Add(new Tuple<string, double>(path + (id[i] <= 9 ? "0" : "") + id[i] + ".png", 4));
            }
            List<BitmapSource> frames = new List<BitmapSource>();
            for (int i = 0; i < data.Count; i++)
            {
                BitmapImage thisIsABitmapImage = new BitmapImage(new Uri(data[i].Item1));
                frames.Add(thisIsABitmapImage);
            }
            MagickImageCollection collection = new MagickImageCollection();
            for (int i = 0; i < frames.Count; i++)
            {
                using (var image = new MagickImage(bitmapToBytes(frames[i])))
                {
                    collection.Add(image.Clone());
                    collection.Last().AnimationDelay = (uint)(Math.Max(data[i].Item2 * 20, 20));
                }
            }
            collection.Optimize();
            collection.Write(path + "GIF_Test1.gif");
            collection.Dispose();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, int>> data = new List<Tuple<string, int>>();
            //open
            string path = @"C:\Users\Admin\TRINSE\TRINSE - Documentos\13.Treinamentos\05.FEM no projeto de trafo\01.Aula demonstrativa\01 GIF\";
            List<int> times = new List<int>();
            for (int i = 1; i <= 14; i++)
            {
                times.Add(15);
            }
            for (int i = 1; i <= times.Count; i++)
            {
                data.Add(new Tuple<string, int>(path + "Screenshot_" + i.ToString() + ".png", times[i - 1]));
            }
            List<BitmapSource> frames = new List<BitmapSource>();
            for (int i = 0; i < data.Count; i++)
            {
                BitmapImage thisIsABitmapImage = new BitmapImage(new Uri(data[i].Item1));
                frames.Add(thisIsABitmapImage);
            }
            MagickImageCollection collection = new MagickImageCollection();
            for (int i = 0; i < frames.Count; i++)
            {
                using (var image = new MagickImage(bitmapToBytes(frames[i])))
                {
                    collection.Add(image.Clone());
                    collection.Last().AnimationDelay = (uint)(data[i].Item2 * 4);
                }
            }
            collection.Optimize();
            collection.Write(path + "GIF_Open1.gif");
            collection.Dispose();
        }
    }
}