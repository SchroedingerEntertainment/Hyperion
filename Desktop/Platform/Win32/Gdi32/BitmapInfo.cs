// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct BitmapInfo
	{
		public BitmapInfoHeader biHeader;
		public int biColors;

		public static BitmapInfo Create()
		{
			BitmapInfo bi = new BitmapInfo();
			bi.biHeader.biSize = Marshal.SizeOf(typeof(BitmapInfoHeader));
			return bi;
		}
	}
}
