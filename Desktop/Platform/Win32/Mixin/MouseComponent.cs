// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    public struct MouseComponent
    {
        private const float wheelDelta = 120.0f;
        private bool mouseFokus;

        [WndProc(WindowMessage.WM_LBUTTONDOWN)]
        [WndProc(WindowMessage.WM_LBUTTONUP)]
        [WndProc(WindowMessage.WM_MBUTTONDOWN)]
        [WndProc(WindowMessage.WM_MBUTTONUP)]
        [WndProc(WindowMessage.WM_RBUTTONDOWN)]
        [WndProc(WindowMessage.WM_RBUTTONUP)]
        [WndProc(WindowMessage.WM_XBUTTONDOWN)]
        [WndProc(WindowMessage.WM_XBUTTONUP)]
        [WndProc(WindowMessage.WM_MOUSEMOVE)]
        [WndProc(WindowMessage.WM_MOUSEWHEEL)]
        [WndProc(WindowMessage.WM_MOUSEHWHEEL)]
        [WndProc(WindowMessage.WM_MOUSELEAVE)]
        public IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_LBUTTONDOWN:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseDown(MouseButton.Left);
                        }
                    }
                    break;
                case WindowMessage.WM_LBUTTONUP:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseUp(MouseButton.Left);
                        }
                    }
                    break;
                case WindowMessage.WM_MBUTTONDOWN:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseDown(MouseButton.Middle);
                        }
                    }
                    break;
                case WindowMessage.WM_MBUTTONUP:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseUp(MouseButton.Middle);
                        }
                    }
                    break;
                case WindowMessage.WM_RBUTTONDOWN:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseDown(MouseButton.Right);
                        }
                    }
                    break;
                case WindowMessage.WM_RBUTTONUP:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseUp(MouseButton.Right);
                        }
                    }
                    break;
                case WindowMessage.WM_XBUTTONDOWN:
                    {
                        IMouseEventTarget eventTarget; if (wParam.HiWord() == 1)
                        {
                            if ((eventTarget = host as IMouseEventTarget) != null)
                                eventTarget.OnMouseDown(MouseButton.XButton1);
                        }
                        else if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseDown(MouseButton.XButton2);
                        }
                    }
                    break;
                case WindowMessage.WM_XBUTTONUP:
                    {
                        IMouseEventTarget eventTarget; if (wParam.HiWord() == 1)
                        {
                            if ((eventTarget = host as IMouseEventTarget) != null)
                                eventTarget.OnMouseUp(MouseButton.XButton1);
                        }
                        else if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseUp(MouseButton.XButton2);
                        }
                    }
                    break;

                case WindowMessage.WM_MOUSEMOVE:
                    {
                        System.Drawing.Point cursor = lParam.ToPoint();
                        IMouseEventTarget eventTarget; if (!mouseFokus)
                        {
                            mouseFokus = true;
                            if ((eventTarget = host as IMouseEventTarget) != null)
                            {
                                eventTarget.OnMouseEnter(cursor);
                            }
                        }
                        if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseMove(cursor);
                        }
                    }
                    break;

                case WindowMessage.WM_MOUSEWHEEL:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseWheel(0, (float)wParam.HiWord() / wheelDelta);
                        }
                    }
                    break;
                case WindowMessage.WM_MOUSEHWHEEL:
                    {
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseWheel((float)wParam.HiWord() / wheelDelta, 0);
                        }
                    }
                    break;

                case WindowMessage.WM_MOUSELEAVE:
                    {
                        mouseFokus = false;
                        IMouseEventTarget eventTarget; if ((eventTarget = host as IMouseEventTarget) != null)
                        {
                            eventTarget.OnMouseLeave();
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
