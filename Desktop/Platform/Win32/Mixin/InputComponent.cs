// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public struct InputComponent
    {
        [WndProc(WindowMessage.WM_INPUT)]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            IInputEventTarget eventTarget; if ((eventTarget = host as IInputEventTarget) != null)
            {
                eventTarget.OnInput(lParam);
            }
            return IntPtr.Zero;
        }
    }
}
