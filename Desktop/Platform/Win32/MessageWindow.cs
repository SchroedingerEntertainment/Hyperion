// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop.Win32
{
    public struct MessageWindow : INative, IDisposable
    {
        public readonly static INative Default;

        private readonly Window.WndProcPtr wndProc;
        private ushort atom;

        [ReadOnly]
        public IntPtr handle;

        public IntPtr Handle
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return handle; }
        }

        static MessageWindow()
        {
            Default = new MessageWindow(null);
        }
        public MessageWindow([Implicit(true)] IWindow host)
        {
            if (host != null)
            {
                wndProc = (hwnd, msg, wParam, lParam) => host.WndProc(host, hwnd, msg, wParam, lParam);
            }
            else wndProc = (hwnd, msg, wParam, lParam) => WndProc(null, hwnd, msg, wParam, lParam);

            WindowClassEx cls = WindowClassEx.Create();
            cls.lpszClassName = Guid.NewGuid().ToString();
            cls.lpfnWndProc = wndProc;

            this.atom = Window.RegisterClassEx(ref cls);
            if (atom != 0)
            {
                this.handle = Window.CreateWindowEx(0, atom, null, 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                if(handle == IntPtr.Zero)
                    throw new InvalidOperationException();
            }
            else throw new InvalidOperationException();
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                if (!Window.DestroyWindow(handle))
                {
                    Window.SendMessage(handle, WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
                }
                handle = IntPtr.Zero;
            }
            if (atom != 0 && Window.UnregisterClass(atom, Window.GetModuleHandle(null)))
            {
                atom = 0;
            }
        }

        [ILGenerator(typeof(WndProcGenerator))]
        public static IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                #region Destroy
                case WindowMessage.WM_DESTROY:
                    {
                        Window.PostMessage(IntPtr.Zero, msg, IntPtr.Zero, hwnd);
                    }
                    break;
                #endregion

                #region TrayIcon
                case WindowMessage.WM_NOTIFY_EVENT:
                    {
                        Window.PostMessage(IntPtr.Zero, (WindowMessage)lParam.LoWord(), wParam, hwnd);
                    }
                    break;
                default: if ((int)msg == Platform.WM_TBRESTART)
                    {
                        Window.PostMessage(IntPtr.Zero, (WindowMessage)Platform.WM_TBRESTART, IntPtr.Zero, hwnd);
                    }
                    break;
                #endregion
            }
            return Window.DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
