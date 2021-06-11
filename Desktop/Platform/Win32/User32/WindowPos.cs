// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPosition
    {
        /// <summary>
        /// Handle to the window.
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// Specifies the position of the window in Z order (front-to-back position).
        /// This member can be a handle to the window behind which this window is placed,
        /// or can be one of the special values listed with the SetWindowPos function.
        /// </summary>
        public IntPtr hwndInsertAfter;
        /// <summary>
        /// Specifies the position of the left edge of the window.
        /// </summary>
        public int x;
        /// <summary>
        /// Specifies the position of the top edge of the window.
        /// </summary>
        public int y;
        /// <summary>
        /// Specifies the window width, in pixels.
        /// </summary>
        public int cx;
        /// <summary>
        /// Specifies the window height, in pixels.
        /// </summary>
        public int cy;
        /// <summary>
        /// Specifies the window position.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public SetWindowPosFlags flags;
    }
}
