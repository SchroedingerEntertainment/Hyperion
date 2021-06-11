// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class Window
    {
        public static string GetWindowText(IntPtr hwnd)
        {
            int length = GetWindowTextLength(hwnd);
            StringBuilder sb = new StringBuilder(length + 1);

            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public static bool ProcessEvent()
        {
            Message msg = new Message();
            if (PeekMessage(ref msg, IntPtr.Zero, 0, 0, 1))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
                return false;
            }
            else return true;
        }
    }
}
