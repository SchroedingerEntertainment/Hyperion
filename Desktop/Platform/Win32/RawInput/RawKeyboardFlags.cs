// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    [Flags]
    public enum RawKeyboardFlags : short
    {
        /// <summary>
        /// The key is down
        /// </summary>
        RI_KEY_MAKE = 0,

        /// <summary>
        /// The key is up
        /// </summary>
        RI_KEY_BREAK = 1,	

        /// <summary>
        /// The scan code has the E0 prefix
        /// </summary>
        RI_KEY_E0 = 2, 	

        /// <summary>
        /// The scan code has the E1 prefix
        /// </summary>
        RI_KEY_E1 = 4
    }
}
