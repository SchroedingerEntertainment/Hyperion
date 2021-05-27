// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial class Window : Surface
    {
        readonly WndProcPtr wndProc;
        ushort atom;
        Message msg;

        public override bool Visible
        {
            get { return false; }
            set { ShowWindow(handle, ShowWindowCommand.SW_SHOW); }
        }

        public Window()
        {
            wndProc = WndProc;
        }
        protected override void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
            {
                DestroyWindow(handle);
                handle = IntPtr.Zero;
            }
            if (atom != 0 && UnregisterClass(atom, Marshal.GetHINSTANCE(typeof(Window).Module)))
            {
                atom = 0;
            }
            base.Dispose(disposing);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override bool Create()
        {
            if (!this)
            {
                return CreateInstance();
            }
            else return false;
        }
        bool CreateInstance()
        {
            IntPtr instance = Marshal.GetHINSTANCE(typeof(Window).Module);
            if (atom == 0)
            {
                WindowClassEx cls = WindowClassEx.Create();
                cls.style = ClassStyles.CS_OWNDC;
                cls.lpfnWndProc = wndProc;
                cls.hInstance = instance;
                cls.hCursor = Cursor.Default;
                cls.hbrBackground = GetStockObject(StockObject.WHITE_BRUSH);
                cls.lpszClassName = Guid.NewGuid().ToString();

                atom = RegisterClassEx(ref cls);
            }
            if (atom != 0)
            {
                handle = CreateWindowEx(0, atom, null, WindowStyles.WS_OVERLAPPEDWINDOW, 0, 0, 300, 300, IntPtr.Zero, IntPtr.Zero, instance, IntPtr.Zero);
                return (handle != IntPtr.Zero);
            }
            else return false;
        }

        IntPtr WndProc(IntPtr handle, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_DESTROY:
                    {
                        this.handle = IntPtr.Zero;
                    }
                    break;
            }
            return DefWindowProc(handle, msg, wParam, lParam);
        }
        
        public override bool ProcessEvent()
        {
            if (PeekMessage(ref msg, handle, 0, 0, 1))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
                return false;
            }
            else return true;
        }






        public override Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override RectangleF Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override RectangleF ClientRect => throw new NotImplementedException();

        public override bool CanResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool TopMost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Icon Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override WindowButons Buttons { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool HasTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool HasBorder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override WindowState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool AppearsInTaskbar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        

        public override PointF PointToClient(PointF pt)
        {
            throw new NotImplementedException();
        }

        public override PointF PointToScreen(PointF pt)
        {
            throw new NotImplementedException();
        }

        
    }
}