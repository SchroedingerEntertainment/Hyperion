// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICE
    {
        [MarshalAs(UnmanagedType.U2)]
        public HidUsagePage UsagePage;
        [MarshalAs(UnmanagedType.U2)]
        public HidUsage Usage;
        public RawInputDeviceFlags Flags;
        public IntPtr WindowHandle;
    }
}
