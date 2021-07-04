// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        public ushort usMakeCode;
        [MarshalAs(UnmanagedType.U2)]
        public RawKeyboardFlags usFlags;
        public ushort usReserverd;
        public ushort usVKey;
        public uint ulMessage;
        public uint ulExtraInformation;
    }
}
