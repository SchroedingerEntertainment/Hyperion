// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public struct TrayIconComponent
    {
        [WndProc(WindowMessage.WM_NOTIFY_EVENT)]
        [TaskbarMessage]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_NOTIFY_EVENT:
                    {
                        ProcessTrayEvent(host, (WindowMessage)lParam.LoWord(), wParam);
                    }
                    break;
                default: if ((int)msg == Platform.WM_TBRESTART)
                    {
                        ProcessTrayEvent(host, (WindowMessage)Platform.WM_TBRESTART, wParam);
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private static void ProcessTrayEvent(IWindow host, WindowMessage msg, IntPtr wParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_LBUTTONUP:
                    {
                        ITrayEventTarget eventTarget; if ((eventTarget = host as ITrayEventTarget) != null && eventTarget.OnTrayEvent(TrayEvent.Click, wParam.ToPoint()))
                        {
                            break;
                        }
                    }
                    goto default;
                case WindowMessage.WM_LBUTTONDBLCLK:
                    {
                        ITrayEventTarget eventTarget; if ((eventTarget = host as ITrayEventTarget) != null && eventTarget.OnTrayEvent(TrayEvent.DoubleClick, wParam.ToPoint()))
                        {
                            break;
                        }
                    }
                    goto default;
                case WindowMessage.WM_CONTEXTMENU:
                case WindowMessage.WM_RBUTTONUP:
                    {
                        ITrayEventTarget eventTarget; if ((eventTarget = host as ITrayEventTarget) != null && eventTarget.OnTrayEvent(TrayEvent.RightClick, wParam.ToPoint()))
                        {
                            break;
                        }
                    }
                    goto default;
                default:
                    {
                        ITrayEventTarget eventTarget; if ((int)msg != Platform.WM_TBRESTART && (eventTarget = host as ITrayEventTarget) != null && eventTarget.OnTrayEvent(TrayEvent.Redraw, System.Drawing.Point.Empty))
                        {
                            Window.PostMessage(IntPtr.Zero, msg, wParam, host.Handle);
                        }
                    }
                    break;
            }
        }
    }
}
