// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop.Win32
{
    internal struct Renderer : IDisposable
    {
        [Access(AccessFlag.Get)]
        public readonly RenderBuffer buffer;

        BitmapInfo bi;

        public Renderer([Generator(GeneratorFlag.Implicit)] IPlatformObject host)
        {
            this.buffer = new RenderBuffer();
            this.bi = new BitmapInfo();
        }
        public void Dispose()
        {
            buffer.Dispose();
        }

        public void OnResize([Generator(GeneratorFlag.Implicit)] IRenderer host, Size size)
        {
            if (buffer.Resize(Math.Max(32, host.ClientRect.Width.NextPowerOfTwo()), Math.Max(32, host.ClientRect.Height.NextPowerOfTwo())))
            {
                bi = BitmapInfo.Create();
                bi.biHeader.biBitCount = 32;
                bi.biHeader.biPlanes = 1;
                bi.biHeader.biWidth = buffer.Dimension.Width;
                bi.biHeader.biHeight = -buffer.Dimension.Height;
                bi.biHeader.biSizeImage = (buffer.Dimension.Width * buffer.Dimension.Height);
            }
        }

        public void OnFlushBuffer([Generator(GeneratorFlag.Implicit)] IPlatformObject host)
        {
            if (buffer)
            {
                using (Graphics g = Graphics.FromHwnd(host.Handle))
                {
                    Window.SetDIBitsToDevice(g.GetHdc(), 0, 0, buffer.Dimension.Width, buffer.Dimension.Height, 0, 0, 0, buffer.Dimension.Height, buffer.Data, ref bi, 0);
                }
            }
        }
    }
}
