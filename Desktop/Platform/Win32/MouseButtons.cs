// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    [Flags]
    public enum MouseButtons : byte
    {
        /// <summary>
        /// The CTRL key is used
        /// </summary>
        MK_CONTROL = 0x0008,

        /// <summary>
        /// The left mouse button is used
        /// </summary>
        MK_LBUTTON = 0x0001,

        /// <summary>
        /// The middle mouse button is used
        /// </summary>
        MK_MBUTTON = 0x0010,

        /// <summary>
        /// The right mouse button is used
        /// </summary>
        MK_RBUTTON = 0x0002,

        /// <summary>
        /// The SHIFT key is used
        /// </summary>
        MK_SHIFT = 0x0004,

        /// <summary>
        /// The first X button is used
        /// </summary>
        MK_XBUTTON1 = 0x0020,

        /// <summary>
        /// The second X button is used
        /// </summary>
        MK_XBUTTON2 = 0x0040
    }
}
