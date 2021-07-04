// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    [Flags]
    public enum RawMouseButton : short
    {
        RI_MOUSE_LEFT_BUTTON_DOWN = 0x0001,
        RI_MOUSE_LEFT_BUTTON_UP = 0x0002,
        RI_MOUSE_RIGHT_BUTTON_DOWN = 0x0004,
        RI_MOUSE_RIGHT_BUTTON_UP = 0x0008,
        RI_MOUSE_MIDDLE_BUTTON_DOWN = 0x0010,
        RI_MOUSE_MIDDLE_BUTTON_UP = 0x0020,
        RI_MOUSE_BUTTON_4_DOWN = 0x0040,
        RI_MOUSE_BUTTON_4_UP = 0x0080,
        RI_MOUSE_BUTTON_5_DOWN = 0x0100,
        RI_MOUSE_BUTTON_5_UP = 0x0200,
        RI_MOUSE_WHEEL = 0x0400,
        RI_MOUSE_HWHEEL = 0x0800,
    }
}
