// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class CursorsExtension
    {
        public static IntPtr ToHandle(this Cursors cursor)
        {
            return new IntPtr((int)cursor);
        }
    }
}
