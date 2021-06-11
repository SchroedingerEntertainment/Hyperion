// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum NotifyMessage : int
    {
        /// <summary>
        /// The taskbar icon is being created
        /// </summary>
        NIM_ADD = 0x00,

        /// <summary>
        /// The settings of the taskbar icon are being updated
        /// </summary>
        NIM_MODIFY = 0x01,

        /// <summary>
        /// The taskbar icon is deleted
        /// </summary>
        NIM_DELETE = 0x02,

        /// <summary>
        /// Focus is returned to the taskbar icon. Currently not in use
        /// </summary>
        NIM_SETFOCUS = 0x03,

        /// <summary>
        /// Shell32.dll version 5.0 and later only. Instructs the taskbar
        /// to behave according to the version number specified in the 
        /// uVersion member of the structure pointed to by lpdata.
        /// This message allows you to specify whether you want the version
        /// 5.0 behavior found on Microsoft Windows 2000 systems, or the
        /// behavior found on earlier Shell versions. The default value for
        /// uVersion is zero, indicating that the original Windows 95 notify
        /// icon behavior should be used
        /// </summary>
        NIM_SETVERSION = 0x04
    }
}
