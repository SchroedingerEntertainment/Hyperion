// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Window
    {
        const string Gdi32 = "gdi32.dll";

        [DllImport(Gdi32)]
        protected static extern IntPtr GetStockObject(StockObject obj);
    }
}
