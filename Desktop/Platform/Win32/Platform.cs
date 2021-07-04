// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class Platform
    {
        public readonly static Version OsVersion;
        public readonly static int WM_TBRESTART;

        private readonly static MessageWindow messageReceiver;
        /// <summary>
        /// 
        /// </summary>
        public static IntPtr MessageReceiver
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return messageReceiver.handle; }
        }

        static Platform()
        {
            OsVersionInfoEx osverinfo = OsVersionInfoEx.Create();
            if (RtlGetVersion(ref osverinfo) == 0)
            {
                OsVersion = new Version(osverinfo.dwMajorVersion, osverinfo.dwMinorVersion, osverinfo.dwBuildNumber);
            }
            else OsVersion = new Version();
            messageReceiver = new MessageWindow(null);

            WM_TBRESTART = Window.RegisterWindowMessage("TaskbarCreated");
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static Exception GetLastWin32Error()
        {
            int errorCode = Marshal.GetLastWin32Error();
            return new ExternalException(System.Runtime.Platform.GetWin32ErrorMessage(errorCode), errorCode);
        }

        public static bool ProcessEvent([Implicit(true)] IPlatform host)
        {
            Message msg = new Message();
            if (Window.PeekMessage(ref msg, IntPtr.Zero, 0, 0, 1))
            {
                if(msg.hwnd == IntPtr.Zero)
                {
                    switch (msg.message)
                    {
                        #region TrayIcon
                        case WindowMessage.WM_LBUTTONUP:
                            {
                                host.OnTrayEvent(unchecked((long)msg.lParam), TrayEvent.Click, msg.wParam.ToPoint());
                            }
                            break;
                        case WindowMessage.WM_LBUTTONDBLCLK:
                            {
                                host.OnTrayEvent(unchecked((long)msg.lParam), TrayEvent.DoubleClick, msg.wParam.ToPoint());
                            }
                            break;
                        case WindowMessage.WM_CONTEXTMENU:
                        case WindowMessage.WM_RBUTTONUP:
                            {
                                host.OnTrayEvent(unchecked((long)msg.lParam), TrayEvent.RightClick, msg.wParam.ToPoint());
                            }
                            break;
                        default: if ((int)msg.message == Platform.WM_TBRESTART)
                            {
                                host.OnTrayEvent(unchecked((long)msg.lParam), TrayEvent.Redraw, System.Drawing.Point.Empty);
                            }
                            break;
                        #endregion
                    }
                }
                Window.TranslateMessage(ref msg);
                Window.DispatchMessage(ref msg);
                return true;
            }
            else return false;
        }
    }
}
