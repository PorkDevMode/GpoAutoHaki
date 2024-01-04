using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using InputInterceptorNS;
using Microsoft.Win32;

class Program
{
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }


    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You MUST run this program as an administrator, Contact me on discord if you have a issue (tag: pork69)\n");


        

            if (!InputInterceptor.CheckDriverInstalled())
            {
                if (InputInterceptor.CheckAdministratorRights())
                {
                    if (!InputInterceptor.InstallDriver())
                    {
                        Console.WriteLine("Failed to install the driver!");
                        System.Threading.Thread.Sleep(5000);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("RUN AS ADMIN TO INSTALL DRIVER");
                    System.Threading.Thread.Sleep(5000);
                    Environment.Exit(1);
                }
            }

        Console.WriteLine("Input driver is installed! Please restart your pc if it does not work!");

        if (!InputInterceptor.Initialize())
        {
            Console.WriteLine("Failed to initialize driver!");
            System.Threading.Thread.Sleep(5000);
            Environment.Exit(1);
        }

        Console.WriteLine("Initialized driver!\n\n");

        using (KeyboardHook hook = new KeyboardHook())
        {
            Console.WriteLine("Press 1 while in program and hover over the target area, then press Enter.\n");

            while (Console.ReadKey().Key != ConsoleKey.D1) { }

            GetCursorPos(out POINT point);
            int targetX = point.X;
            int targetY = point.Y;
            Color capturedColor = GetColorAt(targetX, targetY);

            Console.WriteLine($"Captured coordinates: X={targetX}, Y={targetY}");
            Console.WriteLine($"Captured color: RGB({capturedColor.R}, {capturedColor.G}, {capturedColor.B})");

            while (true)
            {
                    Color currentColor = GetColorAt(targetX, targetY);
                    if (currentColor == capturedColor)
                    {
                        try
                        {
                            hook.SimulateInput("j", 1, 75);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        Console.WriteLine("HAKI ACTIVATED");
                        System.Threading.Thread.Sleep(3000);
                    }
                
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    static Color GetColorAt(int x, int y)
    {
        using (Bitmap screenPixel = new Bitmap(1, 1))
        {
            using (Graphics g = Graphics.FromImage(screenPixel))
            {
                g.CopyFromScreen(x, y, 0, 0, new Size(1, 1));
            }
            return screenPixel.GetPixel(0, 0);
        }
    }
}
