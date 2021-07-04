// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum RawInputType : int
    {
        /// <summary>
        /// Mouse input
        /// </summary>
        Mouse = 0,

        /// <summary>
        /// Keyboard input
        /// </summary>
        Keyboard = 1,

        /// <summary>
        ///  Human interface device input
        /// </summary>
        Hid = 2,

        /// <summary>
        /// Another device that is not the keyboard or the mouse
        /// </summary>
        Other = 3
    }
}
