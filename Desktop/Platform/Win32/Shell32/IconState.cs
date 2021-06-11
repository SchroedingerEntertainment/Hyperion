// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    [Flags]
    public enum IconState : int
    {
        /// <summary>
        /// The icon is visible
        /// </summary>
        NIS_VISIBLE = 0x00,

        /// <summary>
        /// Hide the icon
        /// </summary>
        NIS_HIDDEN = 0x01,

        /// <summary>
        /// The icon resource is shared between multiple icons
        /// </summary>
        NIS_SHAREDICON = 0x02
    }
}
