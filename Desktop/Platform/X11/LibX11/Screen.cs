// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.X11
{
    public partial class Screen
    {
        #if _WIN32
        const string LibX11 = "libX11.dll";
        #else
        const string LibX11 = "libX11.so.6";
        #endif

        [DllImport(LibX11)]
        protected static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(LibX11)]
        protected static extern IntPtr XSynchronize(IntPtr display, bool onoff);
        
        [DllImport(LibX11)]
        protected static extern int XCloseDisplay(IntPtr display);
    }
}
