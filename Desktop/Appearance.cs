// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Appearance : short
    {
        Icon = 0x1,
        Taskbar = 0x2,

        Title = 0x4,
        
        Minimize = 0x8,
        Maximize = 0x10,
        Close = 0x20,
        ButtonMask = Minimize | Maximize | Close,

        Border = 0x40,
        Resizable = 0x80,

        Passive = 0x100,
        Disabled = 0x200,
        TopMost = 0x400
    }
}
