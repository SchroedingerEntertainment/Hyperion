// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// Specifies Desktop Window Manager (DWM) blur-behind properties. Used by the <see cref="DwmEnableBlurBehindWindow(IntPtr, BlurBehind*)"/> function.
    /// </summary>
    public struct BlurBehind
    {
        /// <summary>
        /// A bitwise combination of <see cref="BlurBehindFlags"/> constant values that indicates which of the members of this structure have been set.
        /// </summary>
        public BlurBehindFlags dwFlags;

        /// <summary>
        /// TRUE to register the window handle to DWM blur behind; FALSE to unregister the window handle from DWM blur behind.
        /// </summary>
        public byte fEnable;

        /// <summary>
        /// The region within the client area where the blur behind will be applied. A NULL value will apply the blur behind the entire client area.
        /// </summary>
        public IntPtr hRgnBlur;

        /// <summary>
        /// TRUE if the window's colorization should transition to match the maximized windows; otherwise, FALSE.
        /// </summary>
        public byte fTransitionOnMaximized;

        /// <summary>
        /// Gets or sets a value indicating whether to register the window handle to DWM blur behind;
        /// Use <c>false</c> to unregister the window handle from DWM blur behind.
        /// </summary>
        public bool Enable
        {
            get { return this.fEnable != 0; }
            set { this.fEnable = value ? (byte)1 : (byte)0; }
        }

        /// <summary>
        /// Gets a <see cref="Region"/> object from the <see cref="hRgnBlur"/> handle.
        /// </summary>
        public System.Drawing.Region Region
        {
            get { return System.Drawing.Region.FromHrgn(this.hRgnBlur); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window's colorization should transition to match the maximized windows.
        /// </summary>
        public bool TransitionOnMaximized
        {
            get { return this.fTransitionOnMaximized != 0; }
            set { this.fTransitionOnMaximized = value ? (byte)1 : (byte)0; }
        }
    }
}