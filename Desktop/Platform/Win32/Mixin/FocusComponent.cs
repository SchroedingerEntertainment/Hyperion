// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public struct FocusComponent
    {
        [WndProc(WindowMessage.WM_ACTIVATE)]
        [WndProc(WindowMessage.WM_SETFOCUS)]
        [WndProc(WindowMessage.WM_KILLFOCUS)]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result = Window.DefWindowProc(hwnd, msg, wParam, lParam);
            switch (msg)
            {
                case WindowMessage.WM_ACTIVATE: switch ((ActivationMode)wParam.LoWord())
                    {
                        case ActivationMode.WA_ACTIVE:
                        case ActivationMode.WA_CLICKACTIVE:
                            {
                                IFocusEventTarget eventTarget; if ((eventTarget = host as IFocusEventTarget) != null)
                                {
                                    eventTarget.OnActivated();
                                }
                            }
                            break;
                        case ActivationMode.WA_INACTIVE:
                            {
                                IFocusEventTarget eventTarget; if ((eventTarget = host as IFocusEventTarget) != null)
                                {
                                    eventTarget.OnDeactivate();
                                }
                            }
                            break;
                    }
                    break;
                case WindowMessage.WM_SETFOCUS:
                    {
                        IFocusEventTarget eventTarget; if ((eventTarget = host as IFocusEventTarget) != null)
                        {
                            eventTarget.OnGotFocus();
                        }
                    }
                    break;
                case WindowMessage.WM_KILLFOCUS:
                    {
                        IFocusEventTarget eventTarget; if ((eventTarget = host as IFocusEventTarget) != null)
                        {
                            eventTarget.OnLostFocus();
                        }
                    }
                    break;
            }
            return result;
        }
    }
}
