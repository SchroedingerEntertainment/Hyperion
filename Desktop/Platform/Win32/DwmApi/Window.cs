// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct Window
    {
        const string DwmApi = "dwmapi.dll";

        [DllImport(DwmApi)]
        public static extern int DwmIsCompositionEnabled(out bool enabled);

        [DllImport(DwmApi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern UInt32 DwmEnableBlurBehindWindow(IntPtr hwnd, ref BlurBehind blurBehind);
    }
}
