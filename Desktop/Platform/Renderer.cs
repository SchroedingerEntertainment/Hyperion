// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop
{
    public struct Renderer : IDisposable
    {
        [ReadOnly]
        public readonly RenderBuffer buffer;

        public Renderer([Implicit] INative host)
        {
            this.buffer = new RenderBuffer();
        }
        public void Dispose()
        {
            buffer.Dispose();
        }

        public void OnResize([Implicit(true)] IRenderer host, Size size)
        {
            buffer.Resize(Math.Max(32, host.ClientRect.Width.NextPowerOfTwo()), Math.Max(32, host.ClientRect.Height.NextPowerOfTwo()));
        }

        public void OnFlushBuffer([Implicit(true)] INative host)
        {
            if (host.Handle != IntPtr.Zero)
            {
                using (Graphics g = Graphics.FromHwnd(host.Handle))
                {
                    g.InterpolationMode = InterpolationMode.Low;
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.DrawImageUnscaled(buffer.RenderTarget, 0, 0);
                }
            }
        }
    }
}
