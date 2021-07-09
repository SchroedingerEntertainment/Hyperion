using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    class Program
    {
        internal static string mouse = string.Empty;
        internal static bool close;

        const int FPS = 40;
        const int FrameTime = 1000 / FPS;

        static void Main()
        {
            Bitmap bmp = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.Clear(Color.Purple);

            Icon ico = Icon.FromHandle(bmp.GetHicon());

            /*TrayIcon icon = TrayIcon.Create();
            icon.Icon = ico;
            icon.Visible = true;*/
            
            RectangleF clip = RectangleF.Empty;
            GraphicsPath path = new GraphicsPath();

            Surface surface = Surface.Create();
            int hash = surface.GetHashCode();

            IDisposable sizeChanged = null;
            sizeChanged = Surface.SizeProperty.Where((id) => id.ObjectId == hash).Subscribe(surface, (instance, id) =>
            {
                Surface tmp = (instance as Surface);
                if (tmp != null)
                {
                    using (Graphics g = Graphics.FromImage(tmp.Buffer.RenderTarget))
                    {
                        path.Reset();
                        path.AddString("Hello World", SystemFonts.DefaultFont.FontFamily, (int)FontStyle.Regular, 32, Point.Empty, StringFormat.GenericDefault);
                        RectangleF bounds = path.GetBounds();

                        g.SetClip(clip);
                        g.Clear(Color.Transparent);
                        g.ResetClip();

                        clip = RectangleF.Union(Rectangle.Empty, bounds);
                        clip.Offset((tmp.ClientRect.Width - clip.Width) / 2, (tmp.ClientRect.Height - clip.Height) / 2);
                        clip.Inflate(2, 2);

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TranslateTransform(clip.X, clip.Y);
                        g.FillPath(Brushes.Black, path);

                        tmp.Invalidate();
                    }
                }
                else sizeChanged.Dispose();

            });
            IDisposable closeEvent = null;
            closeEvent = Surface.CloseEvent.Where((id) => id.Sender == hash).Subscribe(surface, (instance, e) =>
            {
                closeEvent.Dispose();
                close = true;

            });
            surface.SetBounds(150, 150, 300, 300);
            surface.Transparency = Desktop.Transparency.Blur;
            surface.Title = "Hello World";
            surface.Icon = ico;
            surface.Visible = true;
            surface.Initialize();
            
            /*long iconId = icon.Id;
            WeakReference<Surface> ic = new WeakReference<Surface>(surface, false);
            IDisposable dp = TrayIcon.RightClickEvent.Where((e) => e.Sender == iconId).Subscribe((e) =>
            {
                Point origin = TrayIcon.RightClickEvent.GetData(ref e);
                surface.SetBounds(origin.X, origin.Y - 300, 100, 300);
                surface.Visible = true;
                surface.SetOrder(true);
                
            });*/

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            while (!close)
            {
                while (MessageInterface.GetNext())
                    ;
                if (surface.Handle != IntPtr.Zero)
                {
                    if (surface.Dirty)
                        surface.Redraw();
                }
                Thread.Sleep(Math.Max(0, FrameTime - (int)sw.ElapsedMilliseconds));
                sw.Reset();
                sw.Start();
            }

            surface.Dispose();
            //icon.Dispose();
        }
    }
}
