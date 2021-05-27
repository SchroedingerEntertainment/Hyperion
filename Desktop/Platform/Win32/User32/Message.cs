// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hwnd;
        public WindowMessage message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point pt;
    }
}
