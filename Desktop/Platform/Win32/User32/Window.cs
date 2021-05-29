// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Window
    {
        public delegate IntPtr WndProcPtr(IntPtr handle, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW")]
        [return: MarshalAs(UnmanagedType.U2)]
        protected static extern ushort RegisterClassEx(ref WindowClassEx wndClass);

        [DllImport(User32, SetLastError = true)]
        protected static extern IntPtr CreateWindowEx(WindowStylesEx styleEx, uint atom, string windowName, WindowStyles style, int x, int y, int width, int height, IntPtr parent, IntPtr menu, IntPtr instance, IntPtr param);

        [DllImport(User32, SetLastError = true)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [DllImport(User32, EntryPoint = "DefWindowProcW")]
        protected extern static IntPtr DefWindowProc(IntPtr handle, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(ref Message message, IntPtr handle, uint minFilter, uint maxFilter, uint removeMessageFilter);

        [DllImport(User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TranslateMessage(ref Message message);
        
        [DllImport(User32, EntryPoint = "DispatchMessageW")]
        public static extern IntPtr DispatchMessage(ref Message message);

        [DllImport(User32)]
        public static extern bool ShowWindow(IntPtr handle, ShowWindowCommand command);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool DestroyWindow(IntPtr handle);
        
        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool UnregisterClass(uint atom, IntPtr instance);
    }
}
