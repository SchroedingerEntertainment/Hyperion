// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using SE.Hyperion.Desktop.Win32;

namespace SE.Hyperion.Drawing.Win32
{
    public struct WindowFrameComponent
    {
        enum BorderDirection : byte
        {
            None = 0,

            Left = 0x1,
            Right = 0x2,
            Top = 0x4,
            Bottom = 0x8,

            TopLeft = Top | Left,
            TopRight = Top | Right,
            BottomLeft = Bottom | Left,
            BottomRight = Bottom | Right
        }

        const int BorderSize = 6; //px

        [WndProc(WindowMessage.WM_NCHITTEST)]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            Surface owner = (host as Surface);
            System.Drawing.Point cursor = owner.PointToClient(lParam.ToPoint());
            System.Drawing.Size size = owner.ClientRect.Size;

            BorderDirection direction = BorderDirection.None;
            if (cursor.X <= BorderSize)
            {
                direction |= BorderDirection.Left;
            }
            else if (cursor.X >= size.Width - BorderSize)
            {
                direction |= BorderDirection.Right;
            }
            if (cursor.Y <= BorderSize)
            {
                direction |= BorderDirection.Top;
            }
            else if (cursor.Y >= size.Height - BorderSize)
            {
                direction |= BorderDirection.Bottom;
            }
            switch (direction)
            {
                case BorderDirection.Left: return HitTestResult.HTLEFT.ToHandle();
                case BorderDirection.Right: return HitTestResult.HTRIGHT.ToHandle();
                case BorderDirection.Top: return HitTestResult.HTTOP.ToHandle();
                case BorderDirection.Bottom: return HitTestResult.HTBOTTOM.ToHandle();
                case BorderDirection.TopLeft: return HitTestResult.HTTOPLEFT.ToHandle();
                case BorderDirection.TopRight: return HitTestResult.HTTOPRIGHT.ToHandle();
                case BorderDirection.BottomLeft: return HitTestResult.HTBOTTOMLEFT.ToHandle();
                case BorderDirection.BottomRight: return HitTestResult.HTBOTTOMRIGHT.ToHandle();
                default: return HitTestResult.HTCAPTION.ToHandle();
            }
        }
    }
}
