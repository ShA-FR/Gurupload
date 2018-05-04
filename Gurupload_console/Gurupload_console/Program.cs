using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Gurupload_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Capturing window..");
                CaptureDesktop(); // Captures all the screens

            }
            catch(Exception ErrorMessage)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Couldn't capture screenshot" + ErrorMessage);


            }

            Console.WriteLine("Screenshot saved to : C:/Users/Public/Documents/Capture-" + DateTime.Now.ToString("yyyy-MM-dd-h-mm-tt") + ".png");
            Thread.Sleep(1000);
            Console.WriteLine("I will now upload the file..");
            UploadImage();
            Console.WriteLine("Upload complete!");

        }


        #region Upload Image
        private static void UploadImage()
        {
            try
            {
                var client = new ImgurClient("1dd3edbb008eae6", "e25728a4e9c674fe22994462521984d86e0172aa");
                var endpoint = new ImageEndpoint(client);
                IImage image;
                using (var fs = new FileStream(@"C:/Users/Public/Documents/Capture-" +DateTime.Now.ToString("yyyy-MM-dd-h-mm-tt") + ".png", FileMode.Open))
                {
                    image = endpoint.UploadImageStreamAsync(fs).GetAwaiter().GetResult();
                }
                Console.WriteLine("Image uploaded. Image Url: " + image.Link);
                Process.Start(image.Link);
                Console.ReadKey();
            }
            catch (ImgurException imgurEx)
            {
                Console.WriteLine("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
            

        }
        #endregion
        #region CaptureDesktop
        private static void CaptureDesktop()
        {
            Rectangle desktopRect = GetDesktopBounds();

            Bitmap bitmap = new Bitmap(desktopRect.Width, desktopRect.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(desktopRect.Location, Point.Empty, bitmap.Size);

            }
            bitmap.Save("C:/Users/Public/Documents/Capture-" +DateTime.Now.ToString("yyyy-MM-dd-h-mm-tt") + ".png");
            Console.WriteLine("Capture complete!");
        }
        private static Rectangle GetDesktopBounds()
        {
            Rectangle result = new Rectangle();
            foreach (Screen screen in Screen.AllScreens)
            {
                result = Rectangle.Union(result, screen.Bounds);
            }

            return result;
        }
    }
    #endregion
}
