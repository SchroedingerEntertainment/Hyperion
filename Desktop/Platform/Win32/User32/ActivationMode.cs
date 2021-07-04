// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum ActivationMode
    {
        /// <summary>
        /// Deactivated
        /// </summary>
        WA_INACTIVE = 0,

        /// <summary>
        /// Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow 
        /// function or by use of the keyboard interface to select the window)
        /// </summary>
        WA_ACTIVE = 1,

        /// <summary>
        /// Activated by a mouse click
        /// </summary>
        WA_CLICKACTIVE = 2
    }
}
