// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public enum MouseButton : byte
    {
        /// <summary>
        /// No button pressed
        /// </summary>
        None = 0,

        /// <summary>
        /// The left mouse button
        /// </summary>
        Left = 1,

        /// <summary>
        /// The middle mouse button
        /// </summary>
        Middle = 2,

        /// <summary>
        /// The right mouse button
        /// </summary>
        Right = 3,

        /// <summary>
        /// The first XButton (XBUTTON1) on Microsoft IntelliMouse Explorer
        /// </summary>
        XButton1 = 4,

        /// <summary>
        /// The second XButton (XBUTTON2) on Microsoft IntelliMouse Explorer
        /// </summary>
        XButton2 = 5,

        /// <summary>
        /// A double click
        /// </summary>
        Double = 0xFF
    }
}
