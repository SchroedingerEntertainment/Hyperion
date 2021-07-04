// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop.X11
{
    public struct TrayIcon : IDisposable
    {
        [ReadOnly]
        public IntPtr handle;

        public void Dispose()
        { }
    }
}