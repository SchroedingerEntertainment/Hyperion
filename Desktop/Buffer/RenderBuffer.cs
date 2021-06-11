// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop
{
    public class RenderBuffer : IDisposable
    {
        PixelBuffer pbuffer;
        Bitmap renderTarget;

        public Bitmap RenderTarget
        {
            get { return renderTarget; }
        }

        public Size Dimension
        {
            get { return renderTarget.Size; }
        }

        public IntPtr Data
        {
            get { return pbuffer; }
        }

        public RenderBuffer()
        {
            this.pbuffer = new PixelBuffer();

        }
        public void Dispose()
        {
            if (pbuffer != null)
            {
                pbuffer.Dispose();
                pbuffer = null;
            }
            if (renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static implicit operator bool(RenderBuffer buffer)
        {
            return buffer.pbuffer;
        }

        public bool Resize(SizeF size)
        {
            return Resize((int)size.Width, (int)size.Height);
        }
        public bool Resize(int width, int height)
        {
            int stride = width * 4;
            int padding = (stride % 4);
            if (padding != 0)
                padding = 4 - padding;

            stride += padding;
            int length = stride * height;

            return Resize(width, height, length, stride);
        }
        public bool Resize(int width, int height, int length, int stride)
        {
            if (pbuffer.Length != length)
            {
                if (renderTarget != null)
                    renderTarget.Dispose();
                if (pbuffer.Length < length || pbuffer.Length / 2 >= length)
                    pbuffer.Resize(length);

                renderTarget = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, pbuffer);
                return true;
            }
            else return false;
        }
    }
}
