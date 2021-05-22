// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Clipboard
    {
        const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true)]
        protected static extern bool OpenClipboard(IntPtr hWndOwner);

        [DllImport(User32, BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern int RegisterClipboardFormat(string format);
        
        [DllImport(User32, BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern int GetClipboardFormatName(int format, StringBuilder lpString, int cchMax);

        [DllImport(User32, SetLastError = true)]
        protected static extern bool EmptyClipboard();

        [DllImport(User32, SetLastError = true)]
        protected static extern IntPtr GetClipboardData(ClipboardFormat uFormat);

        [DllImport(User32, SetLastError = true)]
        protected static extern IntPtr SetClipboardData(ClipboardFormat uFormat, IntPtr hMem);

        [DllImport(User32, SetLastError = true)]
        protected static extern bool CloseClipboard();
    }
}