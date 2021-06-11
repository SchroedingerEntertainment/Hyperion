using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using SE.Hyperion.Desktop;

namespace SE.Hyperion.Drawing
{
    class Program
    {
        /*const int FPS = 60;
        const int FrameTime = 1000 / FPS;

        private readonly static List<Surface> surfaces;
        private static Spinlockʾ surfaceLock;

        static Program()
        {
            surfaces = new List<Surface>();
            surfaceLock = new Spinlockʾ();
        }

        static void Run()
        {
            bool hasMessages;
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                surfaceLock.Lock();
                try
                {
                    do
                    {
                        hasMessages = false;
                        for (int i = 0; i < surfaces.Count; i++)
                        {
                            if (surfaces[i])
                            {
                                hasMessages |= !surfaces[i].ProcessEvent();
                            }
                            else
                            {
                                surfaces.SwapRemove(i);
                                i--;
                            }
                        }
                    }
                    while (hasMessages);
                    for (int i = 0; i < surfaces.Count; i++)
                    {
                        surfaces[i].ProcessRepaint();
                    }
                }
                finally
                {
                    surfaceLock.Release();
                }
                Thread.Sleep(Math.Max(0, FrameTime - (int)sw.ElapsedMilliseconds));
                sw.Reset();
            }
            while (surfaces.Count > 0);
        }*/

        static void Main()
        {
            Surface surface = Surface.Create();
            surface.SetBounds(150, 150, 300, 300);
            surface.Transparency = TransparencyMask.Blur;
            surface.Title = "Hello World";
            surface.Initialize();
            surface.Visible = true;

            Desktop.Win32.TrayIcon icon = new Desktop.Win32.TrayIcon();
            icon.Initialize(surface);
            icon.SetIcon(SystemIcons.WinLogo);

            while (surface.Handle != IntPtr.Zero)
            {
                while (!Desktop.Win32.Window.ProcessEvent())
                    ;
                while (surface.Redraw())
                    ;
                Thread.Sleep(15);
            }

            icon.Dispose();
        }
    }
}
