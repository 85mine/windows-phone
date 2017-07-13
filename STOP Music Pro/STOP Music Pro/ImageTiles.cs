using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace STOP_Music_Pro
{
    class ImageTiles
    {

        public static string image(string _timer, string _imgsource)
        {
                WriteableBitmap writeableBitmap = new WriteableBitmap(400,400);
                //try
                //{
                //    IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
                //    writeableBitmap.SetSource(new IsolatedStorageFileStream("pintimer.png", FileMode.Open, FileAccess.Read, isf));
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                TextBlock timer = null;
                TextBlock hms = null;
                Image imgIcon = null;
                string urlImage = _timer;
                timer = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Text = _timer.Substring(0, 2) + ":" + _timer.Substring(2, 2) + ":" + _timer.Substring(4, 2),
                    FontSize = 60,
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                hms = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Text = "H : M : S",
                    FontSize = 30,
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                ImageSource imgSource = getImageFromIsolatedStorage(_imgsource);

                imgIcon = new Image()
                {
                    Width = 400,
                    Height = 400,
                    Source = imgSource,
                    Stretch = Stretch.UniformToFill
                };
                writeableBitmap.Render(imgIcon, new TranslateTransform() { X = 0, Y = 0 });
                writeableBitmap.Render(timer, new TranslateTransform() { X = 10, Y = 300 });
                writeableBitmap.Render(hms, new TranslateTransform() { X = 15, Y = 360 });
                writeableBitmap.Invalidate();

                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream imageStream = new IsolatedStorageFileStream("Shared/ShellContent/" + urlImage + ".png", FileMode.Create, isf))
                        {
                           // writeableBitmap.SaveJpeg(imageStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
                            writeableBitmap.WritePNG(imageStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

                urlImage = "isostore:/Shared/ShellContent/" + urlImage + ".png";

                return urlImage;   
        }

        private static ImageSource getImageFromIsolatedStorage(string imageName)
        {
            BitmapImage bimg = new BitmapImage();

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = iso.OpenFile(imageName, FileMode.Open, FileAccess.Read))
                {
                    bimg.SetSource(stream);
                }
            }
            return bimg;
        }
    }
}
