// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class Platform
    {
        public delegate void WinEventProcPtr(IntPtr hWinEventHook, WinEventHook @event, IntPtr hwnd, int idObject, int idChild, int dwEventThread, uint dwmsEventTime);

        private const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true)]
        public static extern IntPtr SetWinEventHook(WinEventHook eventMin, WinEventHook eventMax, IntPtr hmodWinEventProc, WinEventProcPtr lpfnWinEventProc, int idProcess, int idThread, WinEventHookFlags dwflags);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

        [DllImport(User32, SetLastError = true)]
        public static extern int GetRawInputData(IntPtr hRawInput, [MarshalAs(UnmanagedType.U4)] RawData uiBehavior, out RawInput pData, [MarshalAs(UnmanagedType.U4)] ref int pcbSize, [MarshalAs(UnmanagedType.U4)] int cbSizeHeader);
    }
}
