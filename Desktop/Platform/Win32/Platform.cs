// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using SE.Mixin;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class Platform
    {
        private readonly static Version osVersion;
        /// <summary>
        /// 
        /// </summary>
        public static Version OsVersion
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return osVersion; }
        }

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
                osVersion = new Version(osverinfo.dwMajorVersion, osverinfo.dwMinorVersion, osverinfo.dwBuildNumber);
            }
            else osVersion = new Version();
            messageReceiver = new MessageWindow(null);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static Exception GetLastWin32Error()
        {
            int errorCode = Marshal.GetLastWin32Error();
            return new ExternalException(System.Runtime.Platform.GetWin32ErrorMessage(errorCode), errorCode);
        }

        public static bool ProcessEvent([Generator(GeneratorFlag.Implicit)] IPlatform host)
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
                        case WindowMessage.WM_RBUTTONUP:
                            {
                                host.OnTrayEvent(unchecked((long)msg.lParam), TrayEvent.RightClick, msg.wParam.ToPoint());
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
