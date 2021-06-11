// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct CreateStruct
    {
        public IntPtr lpCreateParams;
        public IntPtr hInstance;
        public IntPtr hMenu;
        public IntPtr hwndParent;
        public int cy;
        public int cx;
        public int y;
        public int x;
        [MarshalAs(UnmanagedType.U4)]
        public WindowStyles style;
        public IntPtr lpszName;
        public IntPtr lpszClass;
        [MarshalAs(UnmanagedType.U4)]
        public WindowStylesEx dwExStyle;
    }
}
