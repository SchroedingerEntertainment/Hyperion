// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NotifyIconData
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        public IntPtr hWnd;
        [MarshalAs(UnmanagedType.U4)]
        public int uId;
        [MarshalAs(UnmanagedType.U4)]
        public NotifyFlags uFlags;
        [MarshalAs(UnmanagedType.U4)]
        public WindowMessage uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
        [MarshalAs(UnmanagedType.U4)]
        public IconState dwState;
        [MarshalAs(UnmanagedType.U4)]
        public IconState dwStateMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo;
        [MarshalAs(UnmanagedType.U4)]
        public int uTimeoutOrVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle;
        [MarshalAs(UnmanagedType.U4)]
        public InfoFlags dwInfoFlags;
        public Guid guidItem;
        public IntPtr hBalloonIcon;

        public static NotifyIconData Create(IntPtr hwnd, Guid id)
        {
            NotifyIconData data = new NotifyIconData();
            data.cbSize = Marshal.SizeOf(data);
            data.uFlags = NotifyFlags.NIF_GUID;
            data.guidItem = id;
            data.hWnd = hwnd;
            
            return data;
        }
    }
}
