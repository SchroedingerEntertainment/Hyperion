// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct Window
    {
        public delegate IntPtr WndProcPtr(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
        public const int CW_USEDEFAULT = unchecked((int)0x80000000);

        private const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW")]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern ushort RegisterClassEx(ref WindowClassEx wndClass);

        [DllImport(User32, SetLastError = true)]
        public static extern IntPtr CreateWindowEx(WindowStylesEx styleEx, uint atom, string windowName, WindowStyles style, int x, int y, int width, int height, IntPtr parent, IntPtr menu, IntPtr instance, IntPtr param);

        [DllImport(User32, SetLastError = true)]
        public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hwnd, WindowLongIndexFlags nIndex, IntPtr dwNewLong);

        [DllImport(User32, SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hwnd, WindowLongIndexFlags nIndex, int dwNewLong);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, WindowLongIndexFlags nIndex);

        [DllImport(User32, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hwnd, WindowLongIndexFlags nIndex);

        [DllImport(User32, EntryPoint = "DefWindowProcW")]
        public extern static IntPtr DefWindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(ref Message message, IntPtr hWnd, uint minFilter, uint maxFilter, uint removeMessageFilter);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TranslateMessage(ref Message message);
        
        [DllImport(User32, SetLastError = true, EntryPoint = "DispatchMessageW")]
        public static extern IntPtr DispatchMessage(ref Message message);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport(User32, SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport(User32, SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        
        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        
        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport(User32, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand command);

        [DllImport(User32, SetLastError = true, EntryPoint = "MapVirtualKeyW")]
        public static extern int MapVirtualKey([MarshalAs(UnmanagedType.U4)] int uCode, [MarshalAs(UnmanagedType.U4)] VirtualKeyMap uMapType);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);
        
        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterClass(uint atom, IntPtr instance);
    }
}
