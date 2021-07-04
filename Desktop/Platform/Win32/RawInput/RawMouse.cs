// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawMouse
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ButtonData
        {
            [MarshalAs(UnmanagedType.U2)]
            public RawMouseButton usButtonFlags;
            public ushort usButtonData;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public uint ulButtons;
            [FieldOffset(0)]
            public ButtonData Data;
        }

        public ushort usFlags;
        public Union uButtons;
        public uint ulRawButtons;
        public int lLastX;
        public int lLastY;
        public uint ulExtraInformation;
    }
}
