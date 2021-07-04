// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum RawData : int
    {
        /// <summary>
        /// Get the header information from the RAWINPUT structure
        /// </summary>
        RID_HEADER = 0x10000005,

        /// <summary>
        /// Get the raw data from the RAWINPUT structure
        /// </summary>
        RID_INPUT = 0x10000003
    }
}
