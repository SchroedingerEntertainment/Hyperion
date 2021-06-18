// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using SE.Mixin;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct Window : IDisposable
    {
        const Appearance StyleFlags = Appearance.Title | Appearance.Minimize | Appearance.Maximize | Appearance.Close | Appearance.Border | Appearance.Resizable | Appearance.Disabled;

        private readonly WndProcPtr wndProc;
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
        public Appearance appearance;

        [Access(AccessFlag.Get)]
        public Transparency transparency;

        [Access(AccessFlag.Get)]
        public WindowState state;

        [Access(AccessFlag.Get)]
        public string title;

        [Access(AccessFlag.Get)]
        public bool visible;

        public Window([Generator(GeneratorFlag.Implicit)] IWindow host)
        {
            this.wndProc = (hwnd, msg, wParam, lParam) => host.WndProc(host, hwnd, msg, wParam, lParam);
            this.appearance = Appearance.Taskbar | Appearance.Icon | Appearance.Title | Appearance.Border | Appearance.Resizable | Appearance.Minimize | Appearance.Maximize | Appearance.Close;
            this.transparency = Transparency.None;
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
                if (!DestroyWindow(handle))
                {
                    SendMessage(handle, WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
                }
                handle = IntPtr.Zero;
            }
            if (atom != 0 && UnregisterClass(atom, GetModuleHandle(null)))
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

                atom = RegisterClassEx(ref cls);
            }
            if (atom != 0)
            {
                int x = Math.Max(0, bounds.X);
                int y = Math.Max(0, bounds.Y);
                int w = ((bounds.Width <= 0) ? CW_USEDEFAULT : bounds.Width);
                int h = ((bounds.Height <= 0) ? CW_USEDEFAULT : bounds.Height);

                WindowStyles style; if ((appearance & Appearance.Title) == Appearance.Title)
                {
                    style = WindowStyles.WS_CAPTION;
                }
                else style = WindowStyles.WS_POPUP;
                if ((appearance & Appearance.Minimize) == Appearance.Minimize)
                {
                    style |= WindowStyles.WS_MINIMIZEBOX;
                }
                if ((appearance & Appearance.Maximize) == Appearance.Maximize)
                {
                    style |= WindowStyles.WS_MAXIMIZEBOX;
                }
                if ((appearance & Appearance.Close) == Appearance.Close)
                {
                    style |= WindowStyles.WS_SYSMENU;
                }
                if ((appearance & Appearance.Border) == Appearance.Border)
                {
                    style |= WindowStyles.WS_BORDER;
                }
                if ((appearance & Appearance.Resizable) == Appearance.Resizable)
                {
                    style |= WindowStyles.WS_SIZEFRAME;
                }
                if ((appearance & Appearance.Disabled) == Appearance.Disabled)
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

                WindowStylesEx styleEx; if ((appearance & Appearance.Taskbar) == Appearance.Taskbar)
                {
                    styleEx = WindowStylesEx.WS_EX_APPWINDOW;
                }
                else styleEx = WindowStylesEx.Default;
                if ((appearance & Appearance.Icon) != Appearance.Icon)
                {
                    styleEx |= WindowStylesEx.WS_EX_DLGMODALFRAME;
                }
                if ((appearance & Appearance.TopMost) == Appearance.TopMost)
                {
                    styleEx |= WindowStylesEx.WS_EX_TOPMOST;
                }

                handle = CreateWindowEx(styleEx, atom, title, style, x, y, w, h, (appearance & Appearance.Taskbar) != Appearance.Taskbar ? Platform.MessageReceiver : IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                return (handle != IntPtr.Zero);
            }
            else return false;
        }

        public IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
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
                        PostMessage(IntPtr.Zero, msg, IntPtr.Zero, handle);
                        handle = IntPtr.Zero;
                    }
                    break;
                #endregion

                #region TrayIcon
                case WindowMessage.WM_NOTIFY_EVENT:
                    {
                        ProcessTrayEvent(host, (WindowMessage)lParam.LoWord(), wParam);
                    }
                    break;
                #endregion
            }
            try
            {
                result = DefWindowProc(hwnd, msg, wParam, lParam);
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
                        title = GetWindowText(handle);
                        host.OnTitleChanged();
                    }
                    break;
                #endregion
            }
            return result;
        }
        void ProcessWindowStateChange(IWindow host, ResizingRequest request)
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
        void ProcessWindowChange(IWindow host, IntPtr hwnd, IntPtr lParam)
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
                Rect rect; if (GetClientRect(hwnd, out rect))
                {
                    clientRect = rect.ToRectangle();
                }
                host.OnResize(bounds.Size);
            }
            host.OnMove(bounds.Location);
        }
        void ProcessWindowChange(IWindow host, IntPtr lParam)
        {
            WindowPosition pos = (WindowPosition)Marshal.PtrToStructure(lParam, typeof(WindowPosition));
            if (pos.hwnd == handle)
            {
                WindowStylesEx style = (WindowStylesEx)GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                if ((style & WindowStylesEx.WS_EX_TOPMOST) == WindowStylesEx.WS_EX_TOPMOST && (appearance & Appearance.TopMost) != Appearance.TopMost)
                {
                    appearance |= Appearance.TopMost;
                }
                else if ((appearance & Appearance.TopMost) == Appearance.TopMost)
                {
                    appearance &= ~Appearance.TopMost;
                }
                if (state != WindowState.Minimized)
                {
                    bool posChanged = (pos.x != bounds.X || pos.y != bounds.Y);
                    bool sizeChanged = (pos.cx != bounds.Width || pos.cy != bounds.Height);

                    bounds = new Rectangle(pos.x, pos.y, pos.cx, pos.cy);
                    if (sizeChanged)
                    {
                        Rect rect; if (GetClientRect(handle, out rect))
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
                        appearance &= ~StyleFlags;
                        WindowStyles style = (WindowStyles)GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
                        if ((style & WindowStyles.WS_CAPTION) == WindowStyles.WS_CAPTION)
                        {
                            appearance |= Appearance.Title;
                        }
                        if ((style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX)
                        {
                            appearance |= Appearance.Minimize;
                        }
                        if ((style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX)
                        {
                            appearance |= Appearance.Maximize;
                        }
                        if ((style & WindowStyles.WS_SYSMENU) == WindowStyles.WS_SYSMENU)
                        {
                            appearance |= Appearance.Close;
                        }
                        if ((style & WindowStyles.WS_BORDER) == WindowStyles.WS_BORDER)
                        {
                            appearance |= Appearance.Border;
                        }
                        if ((style & WindowStyles.WS_SIZEFRAME) == WindowStyles.WS_SIZEFRAME)
                        {
                            appearance |= Appearance.Resizable;
                        }
                        if ((style & WindowStyles.WS_DISABLED) == WindowStyles.WS_DISABLED)
                        {
                            appearance |= Appearance.Disabled;
                        }
                    }
                    break;
                case WindowLongIndexFlags.GWL_EXSTYLE:
                    {
                        WindowStylesEx style = (WindowStylesEx)GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                        if ((style & WindowStylesEx.WS_EX_DLGMODALFRAME) != WindowStylesEx.WS_EX_DLGMODALFRAME)
                        {
                            appearance |= Appearance.Icon;
                        }
                        else appearance &= ~Appearance.Icon;
                        if ((style & WindowStylesEx.WS_EX_APPWINDOW) == WindowStylesEx.WS_EX_APPWINDOW || GetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT) == IntPtr.Zero)
                        {
                            appearance |= Appearance.Taskbar;
                        }
                        else if (GetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT) != IntPtr.Zero)
                        {
                            appearance &= ~Appearance.Taskbar;
                        }
                    }
                    break;
            }
        }
        void ProcessTrayEvent(IWindow host, WindowMessage msg, IntPtr wParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_LBUTTONUP:
                    {
                        if (host.OnTrayEvent(TrayEvent.Click, wParam.ToPoint()))
                            break;
                    }
                    goto default;
                case WindowMessage.WM_LBUTTONDBLCLK:
                    {
                        if (host.OnTrayEvent(TrayEvent.DoubleClick, wParam.ToPoint()))
                            break;
                    }
                    goto default;
                case WindowMessage.WM_RBUTTONUP:
                    {
                        if (host.OnTrayEvent(TrayEvent.RightClick, wParam.ToPoint()))
                            break;
                    }
                    goto default;
                default:
                    {
                        PostMessage(IntPtr.Zero, msg, wParam, handle);
                    }
                    break;
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetBounds(int x, int y, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                SetWindowPos(handle, IntPtr.Zero, x, y, Math.Max(0, width), Math.Max(0, height), SetWindowPosFlags.SWP_NOZORDER);
            }
            else bounds = new Rectangle(x, y, width, height);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetTitle(string value)
        {
            if (handle != IntPtr.Zero)
            {
                SetWindowText(handle, value);
            }
            else title = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetAppearance(Appearance value)
        {
            if (handle != IntPtr.Zero)
            {
                Appearance diff = value ^ appearance;
                if ((diff & StyleFlags) != 0)
                {
                    WindowStyles style = (WindowStyles)GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
                    if ((value & Appearance.Title) == Appearance.Title)
                    {
                        style &= ~WindowStyles.WS_POPUP;
                        style |= WindowStyles.WS_CAPTION;
                    }
                    else
                    {
                        style &= ~WindowStyles.WS_CAPTION;
                        style |= WindowStyles.WS_POPUP;
                    }
                    if ((value & Appearance.Minimize) == Appearance.Minimize)
                    {
                        style |= WindowStyles.WS_MINIMIZEBOX;
                    }
                    else style &= ~WindowStyles.WS_MINIMIZEBOX;
                    if ((value & Appearance.Maximize) == Appearance.Maximize)
                    {
                        style |= WindowStyles.WS_MAXIMIZEBOX;
                    }
                    else style &= ~WindowStyles.WS_MAXIMIZEBOX;
                    if ((value & Appearance.Close) == Appearance.Close)
                    {
                        style |= WindowStyles.WS_SYSMENU;
                    }
                    else style &= ~WindowStyles.WS_SYSMENU;
                    if ((value & Appearance.Border) == Appearance.Border)
                    {
                        style |= WindowStyles.WS_BORDER;
                    }
                    else style &= ~WindowStyles.WS_BORDER;
                    if ((value & Appearance.Resizable) == Appearance.Resizable)
                    {
                        style |= WindowStyles.WS_SIZEFRAME;
                    }
                    else style &= ~WindowStyles.WS_SIZEFRAME;
                    if ((value & Appearance.Disabled) == Appearance.Disabled)
                    {
                        style |= WindowStyles.WS_DISABLED;
                    }
                    else style &= ~WindowStyles.WS_DISABLED;
                    SetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE, (int)style);
                    SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
                }
                if ((diff & Appearance.Icon) == Appearance.Icon)
                {
                    WindowStylesEx style = (WindowStylesEx)GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                    if ((value & Appearance.Icon) == Appearance.Icon)
                    {
                        style &= ~WindowStylesEx.WS_EX_DLGMODALFRAME;
                    }
                    else style |= WindowStylesEx.WS_EX_DLGMODALFRAME;
                    if (visible)
                    {
                        ShowWindow(handle, ShowWindowCommand.SW_HIDE);
                        ShowWindow(handle, ShowWindowCommand.SW_SHOW);
                    }
                    SetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE, (int)style);
                }
                if ((diff & Appearance.TopMost) == Appearance.TopMost)
                {
                    if ((value & Appearance.TopMost) == Appearance.TopMost)
                    {
                        SetWindowPos(handle, WindowOrder.HWND_TOPMOST.ToHandle(), 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);
                    }
                    else SetWindowPos(handle, WindowOrder.HWND_NOTOPMOST.ToHandle(), 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);
                }
                if ((diff & Appearance.Taskbar) == Appearance.Taskbar)
                {
                    WindowStylesEx style = (WindowStylesEx)GetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE);
                    if ((value & Appearance.Taskbar) == Appearance.Taskbar)
                    {
                        SetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT, IntPtr.Zero);
                        style |= WindowStylesEx.WS_EX_APPWINDOW;
                    }
                    else
                    {
                        SetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT, Platform.MessageReceiver);
                        style &= ~WindowStylesEx.WS_EX_APPWINDOW;
                    }
                    SetWindowLong(handle, WindowLongIndexFlags.GWL_EXSTYLE, (int)style);
                    SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
                }
            }
            else appearance = value;
        }

        public void SetTransparency(Transparency value)
        {
            Transparency tmp = transparency;
            if (handle != IntPtr.Zero)
            {
                if (Platform.OsVersion.Major >= 6)
                {
                    bool enabled; if (DwmIsCompositionEnabled(out enabled) != 0 || !enabled)
                    {
                        transparency = Transparency.None;
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
                else transparency = Transparency.None;
            }
            else transparency = value;
            dirty = (transparency != tmp);
        }
        Transparency SetTransparencyGradient(Transparency mask)
        {
            AccentPolicy policy = new AccentPolicy();

            switch (mask)
            {
                case Transparency.AcrylicBlur:
                    {
                        if (Platform.OsVersion.Major <= 10 && Platform.OsVersion.Build < 19628)
                        {
                            mask = Transparency.Blur;
                            goto case Transparency.Blur;
                        }
                        else policy.AccentState = AccentState.ACCENT_ENABLE_ACRYLIC;
                    }
                    break;
                case Transparency.Blur:
                    {
                        policy.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
                    }
                    break;
                case Transparency.Transparent:
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

            SetWindowCompositionAttribute(handle, ref data);

            Marshal.FreeHGlobal(policyPtr);
            return mask;
        }
        Transparency SetTransparencyComposition(Transparency mask)
        {
            AccentPolicy policy = new AccentPolicy();
            switch (mask)
            {
                case Transparency.AcrylicBlur:
                    {
                        mask = Transparency.Blur;
                    }
                    goto default;
                case Transparency.Transparent:
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

            SetWindowCompositionAttribute(handle, ref data);
            Marshal.FreeHGlobal(policyPtr);

            if (mask == Transparency.Blur)
            {
                SetTransparencyDWM(mask);
            }
            return mask;
        }
        Transparency SetTransparencyDWM(Transparency mask)
        {
            BlurBehind nfo = new BlurBehind();
            switch (mask)
            {
                case Transparency.AcrylicBlur:
                case Transparency.Transparent:
                case Transparency.Blur:
                    {
                        mask = Transparency.Blur;
                        nfo.Enable = true;
                    }
                    goto default;
                default:
                    {
                        DwmEnableBlurBehindWindow(handle, ref nfo);
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
                ShowWindow(handle, cmd);
            }
            else state = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetVisible(bool value)
        {
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, value ? ShowWindowCommand.SW_SHOW : ShowWindowCommand.SW_HIDE);
            }
            else visible = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public System.Drawing.Point PointToClient(System.Drawing.Point pt)
        {
            Point tmp = new Point(pt.X, pt.Y);
            ScreenToClient(handle, ref tmp);
            
            return new System.Drawing.Point(tmp.x, tmp.y);
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public PointF PointToClient(PointF pt)
        {
            Point tmp = new Point((int)pt.X, (int)pt.Y);
            ScreenToClient(handle, ref tmp);
            
            return new PointF(tmp.x, tmp.y);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public System.Drawing.Point PointToScreen(System.Drawing.Point pt)
        {
            Point tmp = new Point(pt.X, pt.Y);
            ClientToScreen(handle, ref tmp);

            return new System.Drawing.Point(tmp.x, tmp.y);
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public PointF PointToScreen(PointF pt)
        {
            Point tmp = new Point((int)pt.X, (int)pt.Y);
            ClientToScreen(handle, ref tmp);

            return new PointF(tmp.x, tmp.y);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Redraw([Generator(GeneratorFlag.Implicit)] IWindow host)
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
            PostMessage(handle, WindowMessage.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public static string GetWindowText(IntPtr hwnd)
        {
            int length = GetWindowTextLength(hwnd);
            StringBuilder sb = new StringBuilder(length + 1);

            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}