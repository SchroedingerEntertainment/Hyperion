// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop.Win32
{
    internal struct Surface : IDisposable
    {
        const SurfaceFlags StyleFlags = SurfaceFlags.Title | SurfaceFlags.Minimize | SurfaceFlags.Maximize | SurfaceFlags.Close | SurfaceFlags.Border | SurfaceFlags.Resizable | SurfaceFlags.Disabled;

        private readonly Window.WndProcPtr wndProc;
        private ushort atom;
        
        [Access(AccessFlag.Get)]
        public IntPtr handle;

        [Access(AccessFlag.Get)]
        public Rectangle bounds;

        [Access(AccessFlag.Get)]
        public Rectangle clientRect;

        [Access(AccessFlag.Get)]
        public bool sizeMove;

        [Access(AccessFlag.Get)]
        public bool dirty;
        
        [Access(AccessFlag.Get)]
        public SurfaceFlags flags;

        [Access(AccessFlag.Get)]
        public TransparencyMask transparency;

        [Access(AccessFlag.Get)]
        public WindowState state;

        [Access(AccessFlag.Get)]
        public string title;

        [Access(AccessFlag.Get)]
        public bool visible;

        public Surface([Generator(GeneratorFlag.Implicit)] IWin32Surface host)
        {
            this.wndProc = (hwnd, msg, wParam, lParam) => host.WndProc(host, hwnd, msg, wParam, lParam);
            this.flags = SurfaceFlags.Taskbar | SurfaceFlags.Icon | SurfaceFlags.Title | SurfaceFlags.Border | SurfaceFlags.Resizable | SurfaceFlags.Minimize | SurfaceFlags.Maximize | SurfaceFlags.Close;
            this.transparency = TransparencyMask.None;
            this.state = WindowState.Normal;
            this.bounds = Rectangle.Empty;
            this.clientRect = Rectangle.Empty;
            this.handle = IntPtr.Zero;
            this.title = string.Empty;
            this.sizeMove = false;
            this.visible = false;
            this.dirty = false;
            this.atom = 0;
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
        
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Initialize()
        {
            if (handle == IntPtr.Zero)
            {
                return CreateInstance();
            }
            else return false;
        }
        bool CreateInstance()
        {
            if (atom == 0)
            {
                WindowClassEx cls = WindowClassEx.Create();
                cls.style = ClassStyles.CS_VREDRAW | ClassStyles.CS_HREDRAW;
                cls.lpfnWndProc = wndProc;
                cls.hCursor = Cursor.Default;
                cls.lpszClassName = Guid.NewGuid().ToString();

                atom = Window.RegisterClassEx(ref cls);
            }
            if (atom != 0)
            {
                int x = Math.Max(0, bounds.X);
                int y = Math.Max(0, bounds.Y);
                int w = ((bounds.Width <= 0) ? Window.CW_USEDEFAULT : bounds.Width);
                int h = ((bounds.Height <= 0) ? Window.CW_USEDEFAULT : bounds.Height);

                WindowStyles style; if ((flags & SurfaceFlags.Title) == SurfaceFlags.Title)
                {
                    style = WindowStyles.WS_CAPTION;
                }
                else style = WindowStyles.WS_POPUP;
                if ((flags & SurfaceFlags.Minimize) == SurfaceFlags.Minimize)
                {
                    style |= WindowStyles.WS_MINIMIZEBOX;
                }
                if ((flags & SurfaceFlags.Maximize) == SurfaceFlags.Maximize)
                {
                    style |= WindowStyles.WS_MAXIMIZEBOX;
                }
                if ((flags & SurfaceFlags.Close) == SurfaceFlags.Close)
                {
                    style |= WindowStyles.WS_SYSMENU;
                }
                if ((flags & SurfaceFlags.Border) == SurfaceFlags.Border)
                {
                    style |= WindowStyles.WS_BORDER;
                }
                if ((flags & SurfaceFlags.Resizable) == SurfaceFlags.Resizable)
                {
                    style |= WindowStyles.WS_SIZEFRAME;
                }
                if ((flags & SurfaceFlags.Disabled) == SurfaceFlags.Disabled)
                {
                    style |= WindowStyles.WS_DISABLED;
                }
                switch (state)
                {
                    case WindowState.Minimized:
                        {
                            style |= WindowStyles.WS_MINIMIZE;
                        }
                        break;
                    case WindowState.Maximized:
                        {
                            style |= WindowStyles.WS_MAXIMIZE;
                        }
                        break;
                }
                if (visible)
                {
                    style |= WindowStyles.WS_VISIBLE;
                }

                WindowStylesEx styleEx; if ((flags & SurfaceFlags.Taskbar) == SurfaceFlags.Taskbar)
                {
                    styleEx = WindowStylesEx.WS_EX_APPWINDOW;
                }
                else styleEx = WindowStylesEx.Default;
                if ((flags & SurfaceFlags.Icon) != SurfaceFlags.Icon)
                {
                    styleEx |= WindowStylesEx.WS_EX_DLGMODALFRAME;
                }
                if ((flags & SurfaceFlags.TopMost) == SurfaceFlags.TopMost)
                {
                    styleEx |= WindowStylesEx.WS_EX_TOPMOST;
                }

                handle = Window.CreateWindowEx(styleEx, atom, title, style, x, y, w, h, (flags & SurfaceFlags.Taskbar) != SurfaceFlags.Taskbar ? Platform.DefaultConnector : IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                return (handle != IntPtr.Zero);
            }
            else return false;
        }

        public IntPtr WndProc(IWin32Surface host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result;
            switch (msg)
            {
                #region WindowChange
                case WindowMessage.WM_CREATE:
                    {
                        ProcessWindowChange(host, hwnd, lParam);
                    }
                    break;
                case WindowMessage.WM_WINDOWPOSCHANGED:
                    {
                        ProcessWindowChange(host, lParam);
                    }
                    break;
                #endregion

                #region WindowStateChange
                case WindowMessage.WM_SIZE:
                    {
                        ProcessWindowStateChange(host, (ResizingRequest)unchecked((long)wParam));
                    }
                    break;
                case WindowMessage.WM_ENTERSIZEMOVE:
                    {
                        sizeMove = true;
                    }
                    break;
                case WindowMessage.WM_EXITSIZEMOVE:
                    {
                        sizeMove = false;
                    }
                    break;
                #endregion

                #region Style
                case WindowMessage.WM_STYLECHANGED:
                    {
                        ProcessFlagsChanged((WindowLongIndexFlags)unchecked((long)wParam));
                    }
                    break;
                #endregion

                #region Draw
                case WindowMessage.WM_PAINT:
                case WindowMessage.WM_SYNCPAINT:
                    {
                        if (!sizeMove)
                        {
                            dirty = true;
                        }
                        else host.OnFlushBuffer();
                    }
                    break;
                #endregion

                #region Destroy
                case WindowMessage.WM_DESTROY:
                    {
                        handle = IntPtr.Zero;
                    }
                    break;
                #endregion
            }
            try
            {
                result = Window.DefWindowProc(hwnd, msg, wParam, lParam);
            }
            catch(Exception)
            {
                return IntPtr.Zero;
            }
            switch (msg)
            {
                #region TitleChange
                case WindowMessage.WM_SETTEXT:
                    {
                        title = Window.GetWindowText(handle);
                        host.OnTitleChanged();
                    }
                    break;
                #endregion
            }
            return result;
        }
        void ProcessWindowStateChange(IWin32Surface host, ResizingRequest request)
        {
            WindowState tmp = state;
            switch (request)
            {
                case ResizingRequest.SIZE_MINIMIZED:
                    {
                        state = WindowState.Minimized;
                    }
                    break;
                case ResizingRequest.SIZE_MAXIMIZED:
                    {
                        state = WindowState.Maximized;
                    }
                    break;
                case ResizingRequest.SIZE_RESTORED:
                    {
                        state = WindowState.Normal;
                    }
                    break;
            }
            if (state != tmp)
            {
                host.OnStateChanged(state);
            }
        }
        void ProcessWindowChange(IWin32Surface host, IntPtr hwnd, IntPtr lParam)
        {
            CreateStruct nfo = (CreateStruct)Marshal.PtrToStructure(lParam, typeof(CreateStruct));
            if (handle == IntPtr.Zero)
            {
                handle = hwnd;

                SetTransparency(transparency);
                host.OnCreated();
            }
            bounds = new Rectangle(nfo.x, nfo.y, nfo.cx, nfo.cy);
            {
                Rect rect; if (Window.GetClientRect(hwnd, out rect))
                {
                    clientRect = rect.ToRectangle();
                }
                host.OnResize(bounds.Size);
            }
            host.OnMove(bounds.Location);
        }
        void ProcessWindowChange(IWin32Surface host, IntPtr lParam)
        {
            WindowPosition pos = (WindowPosition)Marshal.PtrToStructure(lParam, typeof(WindowPosition));
            if (pos.hwnd == handle)
            {
                WindowStylesEx style = (WindowStylesEx)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                if ((style & WindowStylesEx.WS_EX_TOPMOST) == WindowStylesEx.WS_EX_TOPMOST && (flags & SurfaceFlags.TopMost) != SurfaceFlags.TopMost)
                {
                    flags |= SurfaceFlags.TopMost;
                }
                else if ((flags & SurfaceFlags.TopMost) == SurfaceFlags.TopMost)
                {
                    flags &= ~SurfaceFlags.TopMost;
                }
                if (state != WindowState.Minimized)
                {
                    bool posChanged = (pos.x != bounds.X || pos.y != bounds.Y);
                    bool sizeChanged = (pos.cx != bounds.Width || pos.cy != bounds.Height);

                    bounds = new Rectangle(pos.x, pos.y, pos.cx, pos.cy);
                    if (sizeChanged)
                    {
                        Rect rect; if (Window.GetClientRect(handle, out rect))
                        {
                            clientRect = rect.ToRectangle();
                        }
                        host.OnResize(bounds.Size);
                    }
                    if (posChanged)
                    {
                        host.OnMove(bounds.Location);
                    }
                }
                if ((pos.flags & (SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_HIDEWINDOW)) != 0)
                {
                    visible = ((pos.flags & SetWindowPosFlags.SWP_SHOWWINDOW) == SetWindowPosFlags.SWP_SHOWWINDOW);
                    host.OnVisibleChanged(visible);
                }
            }
        }
        void ProcessFlagsChanged(WindowLongIndexFlags index)
        {
            switch (index)
            {
                case WindowLongIndexFlags.GWL_STYLE:
                    {
                        flags &= ~StyleFlags;
                        WindowStyles style = (WindowStyles)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
                        if ((style & WindowStyles.WS_CAPTION) == WindowStyles.WS_CAPTION)
                        {
                            flags |= SurfaceFlags.Title;
                        }
                        if ((style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX)
                        {
                            flags |= SurfaceFlags.Minimize;
                        }
                        if ((style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX)
                        {
                            flags |= SurfaceFlags.Maximize;
                        }
                        if ((style & WindowStyles.WS_SYSMENU) == WindowStyles.WS_SYSMENU)
                        {
                            flags |= SurfaceFlags.Close;
                        }
                        if ((style & WindowStyles.WS_BORDER) == WindowStyles.WS_BORDER)
                        {
                            flags |= SurfaceFlags.Border;
                        }
                        if ((style & WindowStyles.WS_SIZEFRAME) == WindowStyles.WS_SIZEFRAME)
                        {
                            flags |= SurfaceFlags.Resizable;
                        }
                        if ((style & WindowStyles.WS_DISABLED) == WindowStyles.WS_DISABLED)
                        {
                            flags |= SurfaceFlags.Disabled;
                        }
                    }
                    break;
                case WindowLongIndexFlags.GWL_EXSTYLE:
                    {
                        WindowStylesEx style = (WindowStylesEx)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                        if ((style & WindowStylesEx.WS_EX_DLGMODALFRAME) != WindowStylesEx.WS_EX_DLGMODALFRAME)
                        {
                            flags |= SurfaceFlags.Icon;
                        }
                        else flags &= ~SurfaceFlags.Icon;
                        if ((style & WindowStylesEx.WS_EX_APPWINDOW) == WindowStylesEx.WS_EX_APPWINDOW || Window.GetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT) == IntPtr.Zero)
                        {
                            flags |= SurfaceFlags.Taskbar;
                        }
                        else if (Window.GetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT) != IntPtr.Zero)
                        {
                            flags &= ~SurfaceFlags.Taskbar;
                        }
                    }
                    break;
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetBounds(int x, int y, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                Window.SetWindowPos(handle, IntPtr.Zero, x, y, Math.Max(0, width), Math.Max(0, height), SetWindowPosFlags.SWP_NOZORDER);
            }
            else bounds = new Rectangle(x, y, width, height);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetTitle(string value)
        {
            if (handle != IntPtr.Zero)
            {
                Window.SetWindowText(handle, value);
            }
            else title = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetFlags(SurfaceFlags value)
        {
            if (handle != IntPtr.Zero)
            {
                SurfaceFlags diff = value ^ flags;
                if ((diff & StyleFlags) != 0)
                {
                    WindowStyles style = (WindowStyles)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
                    if ((value & SurfaceFlags.Title) == SurfaceFlags.Title)
                    {
                        style &= ~WindowStyles.WS_POPUP;
                        style |= WindowStyles.WS_CAPTION;
                    }
                    else
                    {
                        style &= ~WindowStyles.WS_CAPTION;
                        style |= WindowStyles.WS_POPUP;
                    }
                    if ((value & SurfaceFlags.Minimize) == SurfaceFlags.Minimize)
                    {
                        style |= WindowStyles.WS_MINIMIZEBOX;
                    }
                    else style &= ~WindowStyles.WS_MINIMIZEBOX;
                    if ((value & SurfaceFlags.Maximize) == SurfaceFlags.Maximize)
                    {
                        style |= WindowStyles.WS_MAXIMIZEBOX;
                    }
                    else style &= ~WindowStyles.WS_MAXIMIZEBOX;
                    if ((value & SurfaceFlags.Close) == SurfaceFlags.Close)
                    {
                        style |= WindowStyles.WS_SYSMENU;
                    }
                    else style &= ~WindowStyles.WS_SYSMENU;
                    if ((value & SurfaceFlags.Border) == SurfaceFlags.Border)
                    {
                        style |= WindowStyles.WS_BORDER;
                    }
                    else style &= ~WindowStyles.WS_BORDER;
                    if ((value & SurfaceFlags.Resizable) == SurfaceFlags.Resizable)
                    {
                        style |= WindowStyles.WS_SIZEFRAME;
                    }
                    else style &= ~WindowStyles.WS_SIZEFRAME;
                    if ((value & SurfaceFlags.Disabled) == SurfaceFlags.Disabled)
                    {
                        style |= WindowStyles.WS_DISABLED;
                    }
                    else style &= ~WindowStyles.WS_DISABLED;
                    Window.SetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE, (int)style);
                    Window.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
                }
                if ((diff & SurfaceFlags.Icon) == SurfaceFlags.Icon)
                {
                    WindowStylesEx style = (WindowStylesEx)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                    if ((value & SurfaceFlags.Icon) == SurfaceFlags.Icon)
                    {
                        style &= ~WindowStylesEx.WS_EX_DLGMODALFRAME;
                    }
                    else style |= WindowStylesEx.WS_EX_DLGMODALFRAME;
                    if (visible)
                    {
                        Window.ShowWindow(handle, ShowWindowCommand.SW_HIDE);
                        Window.ShowWindow(handle, ShowWindowCommand.SW_SHOW);
                    }
                    Window.SetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE, (int)style);
                }
                if ((diff & SurfaceFlags.TopMost) == SurfaceFlags.TopMost)
                {
                    if ((value & SurfaceFlags.TopMost) == SurfaceFlags.TopMost)
                    {
                        Window.SetWindowPos(handle, WindowOrder.HWND_TOPMOST.ToHandle(), 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);
                    }
                    else Window.SetWindowPos(handle, WindowOrder.HWND_NOTOPMOST.ToHandle(), 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);
                }
                if ((diff & SurfaceFlags.Taskbar) == SurfaceFlags.Taskbar)
                {
                    WindowStylesEx style = (WindowStylesEx)Window.GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                    if ((value & SurfaceFlags.Taskbar) == SurfaceFlags.Taskbar)
                    {
                        Window.SetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT, IntPtr.Zero);
                        style |= WindowStylesEx.WS_EX_APPWINDOW;
                    }
                    else
                    {
                        Window.SetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT, Platform.DefaultConnector);
                        style &= ~WindowStylesEx.WS_EX_APPWINDOW;
                    }
                    Window.SetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE, (int)style);
                    Window.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
                }
            }
            else flags = value;
        }

        public void SetTransparency(TransparencyMask value)
        {
            TransparencyMask tmp = transparency;
            if (handle != IntPtr.Zero)
            {
                if (Platform.OsVersion.Major >= 6)
                {
                    bool enabled; if (Window.DwmIsCompositionEnabled(out enabled) != 0 || !enabled)
                    {
                        transparency = TransparencyMask.None;
                    }
                    else if (Platform.OsVersion.Major >= 10)
                    {
                        transparency = SetTransparencyGradient(value);
                    }
                    else if (Platform.OsVersion.Minor >= 2)
                    {
                        transparency = SetTransparencyComposition(value);
                    }
                    else transparency = SetTransparencyDWM(value);
                }
                else transparency = TransparencyMask.None;
            }
            else transparency = value;
            dirty = (transparency != tmp);
        }
        TransparencyMask SetTransparencyGradient(TransparencyMask mask)
        {
            AccentPolicy policy = new AccentPolicy();

            switch (mask)
            {
                case TransparencyMask.AcrylicBlur:
                    {
                        if (Platform.OsVersion.Major <= 10 && Platform.OsVersion.Build < 19628)
                        {
                            mask = TransparencyMask.Blur;
                            goto case TransparencyMask.Blur;
                        }
                        else policy.AccentState = AccentState.ACCENT_ENABLE_ACRYLIC;
                    }
                    break;
                case TransparencyMask.Blur:
                    {
                        policy.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
                    }
                    break;
                case TransparencyMask.Transparent:
                    {
                        policy.AccentState = AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;
                    }
                    break;
                default:
                    {
                        policy.AccentState = AccentState.ACCENT_DISABLED;
                    }
                    break;
            }

            policy.AccentFlags = 2;
            policy.GradientColor = 0x01000000;

            int policySize = Marshal.SizeOf(policy);
            IntPtr policyPtr = Marshal.AllocHGlobal(policySize);
            Marshal.StructureToPtr(policy, policyPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = policySize;
            data.Data = policyPtr;

            Window.SetWindowCompositionAttribute(handle, ref data);

            Marshal.FreeHGlobal(policyPtr);
            return mask;
        }
        TransparencyMask SetTransparencyComposition(TransparencyMask mask)
        {
            AccentPolicy policy = new AccentPolicy();
            switch (mask)
            {
                case TransparencyMask.AcrylicBlur:
                    {
                        mask = TransparencyMask.Blur;
                    }
                    goto default;
                case TransparencyMask.Transparent:
                    {
                        policy.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
                    }
                    break;
                default:
                    {
                        policy.AccentState = AccentState.ACCENT_DISABLED;
                    }
                    break;
            }

            int policySize = Marshal.SizeOf(policy);
            IntPtr policyPtr = Marshal.AllocHGlobal(policySize);
            Marshal.StructureToPtr(policy, policyPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = policySize;
            data.Data = policyPtr;

            Window.SetWindowCompositionAttribute(handle, ref data);
            Marshal.FreeHGlobal(policyPtr);

            if (mask == TransparencyMask.Blur)
            {
                SetTransparencyDWM(mask);
            }
            return mask;
        }
        TransparencyMask SetTransparencyDWM(TransparencyMask mask)
        {
            BlurBehind nfo = new BlurBehind();
            switch (mask)
            {
                case TransparencyMask.AcrylicBlur:
                case TransparencyMask.Transparent:
                case TransparencyMask.Blur:
                    {
                        mask = TransparencyMask.Blur;
                        nfo.Enable = true;
                    }
                    goto default;
                default:
                    {
                        Window.DwmEnableBlurBehindWindow(handle, ref nfo);
                    }
                    return mask;
            }
        }
        
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetState(WindowState value)
        {
            if (handle != IntPtr.Zero)
            {
                ShowWindowCommand cmd; switch (state)
                {
                    case WindowState.Minimized:
                        {
                            cmd = ShowWindowCommand.SW_MINIMIZE;
                        }
                        break;
                    case WindowState.Maximized:
                        {
                            cmd = ShowWindowCommand.SW_MAXIMIZE;
                        }
                        break;
                    default:
                        {
                            cmd = ShowWindowCommand.SW_RESTORE;
                        }
                        break;
                }
                Window.ShowWindow(handle, cmd);
            }
            else state = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetVisible(bool value)
        {
            if (handle != IntPtr.Zero)
            {
                Window.ShowWindow(handle, value ? ShowWindowCommand.SW_SHOW : ShowWindowCommand.SW_HIDE);
            }
            else visible = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public System.Drawing.Point PointToClient(System.Drawing.Point pt)
        {
            Point tmp = new Point(pt.X, pt.Y);
            Window.ScreenToClient(handle, ref tmp);
            
            return new System.Drawing.Point(tmp.x, tmp.y);
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public PointF PointToClient(PointF pt)
        {
            Point tmp = new Point((int)pt.X, (int)pt.Y);
            Window.ScreenToClient(handle, ref tmp);
            
            return new PointF(tmp.x, tmp.y);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public System.Drawing.Point PointToScreen(System.Drawing.Point pt)
        {
            Point tmp = new Point(pt.X, pt.Y);
            Window.ClientToScreen(handle, ref tmp);

            return new System.Drawing.Point(tmp.x, tmp.y);
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public PointF PointToScreen(PointF pt)
        {
            Point tmp = new Point((int)pt.X, (int)pt.Y);
            Window.ClientToScreen(handle, ref tmp);

            return new PointF(tmp.x, tmp.y);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Redraw([Generator(GeneratorFlag.Implicit)] IWin32Surface host)
        {
            if (dirty)
            {
                dirty = false;
                host.OnFlushBuffer();
            }
            return dirty;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void Close()
        {
            Window.PostMessage(handle, WindowMessage.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}