// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct BitmapInfoHeader
	{
        [MarshalAs(UnmanagedType.U4)]
        public int biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        [MarshalAs(UnmanagedType.U4)]
        public int biCompression;
        [MarshalAs(UnmanagedType.U4)]
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public static BitmapInfoHeader Create()
        {
            BitmapInfoHeader bih = new BitmapInfoHeader();
            bih.biSize = Marshal.SizeOf(typeof(BitmapInfoHeader));
            return bih;
        }
    }
}
