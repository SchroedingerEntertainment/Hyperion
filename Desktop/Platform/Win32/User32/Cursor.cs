// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Cursor
    {
        const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true)]
        protected static extern IntPtr LoadCursor(IntPtr instance, IntPtr cursorName);
    }
}