// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public enum InfoFlags
    {
        /// <summary>
        /// No icon is displayed
        /// </summary>
        NIIF_NONE = 0x00,

        /// <summary>
        /// An information icon is displayed
        /// </summary>
        NIIF_INFO = 0x01,

        /// <summary>
        /// A warning icon is displayed
        /// </summary>
        NIIF_WARNING = 0x02,

        /// <summary>
        /// An error icon is displayed
        /// </summary>
        NIIF_ERROR = 0x03,

        /// <summary>
        /// Windows XP Service Pack 2 (SP2) and later.
        /// Use a custom icon as the title icon
        /// </summary>
        NIIF_USER = 0x04,

        /// <summary>
        /// Windows XP (Shell32.dll version 6.0) and later.
        /// Do not play the associated sound. Applies only to balloon ToolTips
        /// </summary>
        NIIF_NOSOUND = 0x10,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. The large version
        /// of the icon should be used as the balloon icon. This corresponds to the
        /// icon with dimensions SM_CXICON x SM_CYICON. If this flag is not set,
        /// the icon with dimensions XM_CXSMICON x SM_CYSMICON is used.<br/>
        /// - This flag can be used with all stock icons.<br/>
        /// - Applications that use older customized icons (NIIF_USER with hIcon) must
        ///   provide a new SM_CXICON x SM_CYICON version in the tray icon (hIcon). These
        ///   icons are scaled down when they are displayed in the System Tray or
        ///   System Control Area (SCA).<br/>
        /// - New customized icons (NIIF_USER with hBalloonIcon) must supply an
        ///   SM_CXICON x SM_CYICON version in the supplied icon (hBalloonIcon)
        /// </summary>
        NIIF_LARGE_ICON = 0x20,

        /// <summary>
        /// Windows 7 and later. Do not display the balloon notification if the current 
        /// user is in "quiet time"
        /// </summary>
        NIIF_RESPECT_QUIET_TIME = 0x80
    }
}
