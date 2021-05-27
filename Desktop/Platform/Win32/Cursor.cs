// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Cursor
    {
        private readonly static IntPtr @default;
        /// <summary>
        /// 
        /// </summary>
        public static IntPtr Default
        {
            get { return @default; }
        }

        static Cursor()
        {
            @default = LoadCursor(IntPtr.Zero, Cursors.IDC_ARROW.ToHandle());
        }
    }
}
