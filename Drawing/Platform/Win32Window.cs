// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SE.Hyperion.Desktop.Win32;

namespace SE.Hyperion.Drawing.Platform
{
    public class Win32Window : Window
    {
        readonly RenderBuffer buffer;

        public Win32Window()
        {
            this.buffer = new RenderBuffer();
            buffer.Resize(300, 300);
        }

        protected override void OnFlushBuffer()
        {
            using (Graphics g = this)
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
