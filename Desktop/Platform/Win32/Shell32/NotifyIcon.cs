// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class NotifyIcon
    {
        private const string Shell32 = "shell32.dll";

        [DllImport(Shell32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool Shell_NotifyIcon(NotifyMessage dwMessage, ref NotifyIconData data);
    }
}
