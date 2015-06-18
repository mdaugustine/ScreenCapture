using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using AviFile;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch(); //Diagnostic for testing run time
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;

            //Set up the screenshots
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmpScreenshot);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            bmpScreenshot.Save("..\\..\\testdata\\NewScreenshot.bmp", ImageFormat.Bmp);

            //Create the videostream
            AviManager aviManager = new AviManager(@"..\..\testdata\new.avi", false);
            VideoStream aviStream = aviManager.AddVideoStream(true, 25, bmpScreenshot);

            //Capture the screen into bitmaps, captures at about a rate of 20 fps
            List<Bitmap> bitmaps = new List<Bitmap>();
            stopWatch.Start(); //Start the stopwatch
            for (int i = 0; i < 100; i++)
            {
                bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                g = Graphics.FromImage(bmpScreenshot);
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                bitmaps.Add(bmpScreenshot);

                Console.WriteLine("frame: " + i);
            }
            stopWatch.Stop(); //Get the run time

            //Convert the bitmaps to avi
            for (int i = 0; i < bitmaps.Count; i++)
            {
                aviStream.AddFrame(bitmaps[i]);
                Console.WriteLine("Compressing: " + i);
            }

            //Display how much time it took to record
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.Read();

            aviManager.Close();
        }
    }
}
