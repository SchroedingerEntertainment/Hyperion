// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WindowClassEx
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public ClassStyles style;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Window.WndProcPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;

        public static WindowClassEx Create()
        {
            WindowClassEx cls = new WindowClassEx();
            cls.cbSize = Marshal.SizeOf(typeof(WindowClassEx));
            return cls;
        }
    }
}
