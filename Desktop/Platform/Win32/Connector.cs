// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop.Win32
{
    internal struct Connector : IDisposable
    {
        private readonly Window.WndProcPtr wndProc;
        private ushort atom;

        [Access(AccessFlag.Get)]
        public IntPtr handle;

        public Connector([Generator(GeneratorFlag.Implicit)] IWin32Surface host)
        {
            this.wndProc = null;

            WindowClassEx cls = WindowClassEx.Create();
            cls.lpszClassName = Guid.NewGuid().ToString();
            cls.lpfnWndProc = Window.DefWindowProc;

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

        public IntPtr WndProc(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            return Window.DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
