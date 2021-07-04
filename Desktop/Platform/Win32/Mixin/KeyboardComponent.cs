// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public struct KeyboardComponent
    {
        [WndProc(WindowMessage.WM_KEYDOWN)]
        [WndProc(WindowMessage.WM_SYSKEYDOWN)]
        [WndProc(WindowMessage.WM_KEYUP)]
        [WndProc(WindowMessage.WM_SYSKEYUP)]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_KEYDOWN:
                case WindowMessage.WM_SYSKEYDOWN:
                    {
                        IKeyboardEventTarget eventTarget; if ((eventTarget = host as IKeyboardEventTarget) != null)
                        {
                            eventTarget.OnKeyDown(Window.GetKey((int)unchecked((long)wParam), (int)unchecked((long)lParam)));
                        }
                    }
                    break;
                case WindowMessage.WM_KEYUP:
                case WindowMessage.WM_SYSKEYUP:
                    {
                        IKeyboardEventTarget eventTarget; if ((eventTarget = host as IKeyboardEventTarget) != null)
                        {
                            eventTarget.OnKeyUp(Window.GetKey((int)unchecked((long)wParam), (int)unchecked((long)lParam)));
                        }
                    }
                    break;
            }
            return Window.DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
