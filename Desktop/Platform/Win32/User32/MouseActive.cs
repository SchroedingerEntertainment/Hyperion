// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum MouseActive : int
    {
        /// <summary>
        /// Activates the window, and does not discard the mouse message
        /// </summary>
        MA_ACTIVATE = 1,

        /// <summary>
        /// Activates the window, and discards the mouse message
        /// </summary>
        MA_ACTIVATEANDEAT = 2,

        /// <summary>
        /// Does not activate the window, and does not discard the mouse message
        /// </summary>
        MA_NOACTIVATE = 3,

        /// <summary>
        /// Does not activate the window, but discards the mouse message
        /// </summary>
        MA_NOACTIVATEANDEAT = 4
    }
}
