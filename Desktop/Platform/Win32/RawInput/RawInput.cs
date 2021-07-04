// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInput
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public RawMouse Mouse;
            [FieldOffset(0)]
            public RawKeyboard Keyboard;
            [FieldOffset(0)]
            public IntPtr Hid;
        }

        public RawInputHeader Header;
        public Union Data;
    }
}
