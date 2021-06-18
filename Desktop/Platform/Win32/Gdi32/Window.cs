// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct Window
    {
        const string Gdi32 = "gdi32.dll";

        [DllImport(Gdi32)]
        public static extern IntPtr GetStockObject(StockObject obj);

        [DllImport(Gdi32, SetLastError = true)]
        public static extern int SetDIBitsToDevice(IntPtr hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, IntPtr lpvBits, ref BitmapInfo lpbmi, uint fuColorUse);
    }
}
