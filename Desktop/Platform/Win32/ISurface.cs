// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IWin32Surface : ISurfaceEvents, IRendererEvents, IWindowEvents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        IntPtr WndProc(IWin32Surface host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
    }
}
