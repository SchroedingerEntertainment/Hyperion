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

        [ReadOnly]
        public IntPtr handle;

        [ReadOnly]
        public Rectangle bounds;

        [ReadOnly]
        public Rectangle clientRect;

        [ReadOnly]
        public bool sizeMove;

        [ReadOnly]
        public bool dirty;

        public Icon icon;
        public Appearance appearance;
        public Transparency transparency;
        public WindowState state;
        public string title;
        public bool visible;

        public Window([Implicit(true)] IWindow host)
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
            this.icon = null;
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

                if (icon != null)
                {
                    cls.hIcon = icon.Handle;
                    cls.hIconSm = icon.Handle;
                }

                atom = RegisterClassEx(ref cls);
            }
            if (atom != 0)
            {
                int x = Math.Max(0, bounds.X);
                int y = Math.Max(0, bounds.Y);
                int w = ((bounds.Width <= 0) ? CW_USEDEFAULT : bounds.Width);
                int h = ((bounds.Height <= 0) ? CW_USEDEFAULT : bounds.Height);

                WindowStyles style = WindowStyles.WS_POPUP; 
                if ((appearance & Appearance.Title) == Appearance.Title)
                {
                    style |= WindowStyles.WS_CAPTION;
                }
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
                if ((appearance & Appearance.Passive) == Appearance.Passive)
                {
                    styleEx |= WindowStylesEx.WS_EX_NOACTIVATE;
                }

                handle = CreateWindowEx(styleEx, atom, title, style, x, y, w, h, (appearance & Appearance.Taskbar) != Appearance.Taskbar ? MessageWindow.Default.Handle : IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                return (handle != IntPtr.Zero);
            }
            else return false;
        }

        [ILGenerator(typeof(WndProcGenerator))]
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
                case WindowMessage.WM_DPICHANGED:
                    {
                        ProcessDpiChanged(wParam.HiWord(), (Rect)Marshal.PtrToStructure(lParam, typeof(Rect)));
                    }
                    break;
                #endregion

                #region Draw
                case WindowMessage.WM_PAINT:
                case WindowMessage.WM_SYNCPAINT:
                    {
                        IRendererEventTarget eventTarget; if (!sizeMove)
                        {
                            dirty = true;
                        }
                        else if((eventTarget = host as IRendererEventTarget) != null)
                        {
                            eventTarget.OnFlushBuffer();
                        }
                    }
                    break;
                #endregion

                #region Destroy
                case WindowMessage.WM_DESTROY:
                    {
                        ISurfaceEventTarget eventTarget; if ((eventTarget = host as ISurfaceEventTarget) != null)
                        {
                            eventTarget.OnClose();
                        }
                        PostMessage(IntPtr.Zero, msg, IntPtr.Zero, handle);
                        handle = IntPtr.Zero;
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
                        IAppearanceEventTarget eventTarget; if ((eventTarget = host as IAppearanceEventTarget) != null)
                        {
                            eventTarget.OnTitleChanged(title);
                        }
                    }
                    break;
                #endregion

                #region Style
                case WindowMessage.WM_SETICON:
                    {
                        ProcessIconChanged(host, lParam);
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
            IAppearanceEventTarget eventTarget; if (state != tmp && (eventTarget = host as IAppearanceEventTarget) != null)
            {
                eventTarget.OnStateChanged(state);
            }
        }
        void ProcessWindowChange(IWindow host, IntPtr hwnd, IntPtr lParam)
        {
            CreateStruct nfo = (CreateStruct)Marshal.PtrToStructure(lParam, typeof(CreateStruct));
            ISurfaceEventTarget eventTarget = host as ISurfaceEventTarget;
            if (handle == IntPtr.Zero)
            {
                handle = hwnd;

                SetTransparency(transparency);
                if (eventTarget != null)
                {
                    eventTarget.OnCreated();
                }
            }
            bounds = new Rectangle(nfo.x, nfo.y, nfo.cx, nfo.cy);
            {
                Rect rect; if (GetClientRect(hwnd, out rect))
                {
                    clientRect = rect.ToRectangle();
                }
                if (eventTarget != null)
                {
                    eventTarget.OnResize(bounds.Size);
                }
            }
            if (eventTarget != null)
            {
                eventTarget.OnMove(bounds.Location);
            }
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
                ISurfaceEventTarget eventTarget;
                {
                    bool posChanged = (pos.x != bounds.X || pos.y != bounds.Y) && ((pos.flags & SetWindowPosFlags.SWP_NOMOVE) != SetWindowPosFlags.SWP_NOMOVE);
                    bool sizeChanged = (pos.cx != bounds.Width || pos.cy != bounds.Height) && ((pos.flags & SetWindowPosFlags.SWP_NOSIZE) != SetWindowPosFlags.SWP_NOSIZE);

                    if (sizeChanged)
                    {
                        bounds.Size = new Size(pos.cx, pos.cy);
                        Rect rect; if (GetClientRect(handle, out rect))
                        {
                            clientRect = rect.ToRectangle();
                        }
                        if ((eventTarget = host as ISurfaceEventTarget) != null)
                        {
                            eventTarget.OnResize(bounds.Size);
                        }
                    }
                    if (posChanged)
                    {
                        bounds.Location = new System.Drawing.Point(pos.x, pos.y);
                        if ((eventTarget = host as ISurfaceEventTarget) != null)
                        {
                            eventTarget.OnMove(bounds.Location);
                        }
                    }
                }
                if ((pos.flags & (SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_HIDEWINDOW)) != 0 && (eventTarget = host as ISurfaceEventTarget) != null)
                {
                    visible = ((pos.flags & SetWindowPosFlags.SWP_SHOWWINDOW) == SetWindowPosFlags.SWP_SHOWWINDOW);
                    eventTarget.OnVisibleChanged(visible);
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
                        if ((style & WindowStylesEx.WS_EX_NOACTIVATE) == WindowStylesEx.WS_EX_NOACTIVATE)
                        {
                            appearance |= Appearance.Passive;
                        }
                        else appearance &= ~Appearance.Passive;
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
        void ProcessDpiChanged(int dpi, Rect rect)
        {
            Rectangle bounds = rect.ToRectangle();
            SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
        void ProcessIconChanged(IWindow host, IntPtr lParam)
        {
            if ((icon == null && lParam != IntPtr.Zero) || (icon != null && icon.Handle != lParam))
            {
                icon = Icon.FromHandle(lParam);
                IAppearanceEventTarget eventTarget; if ((eventTarget = host as IAppearanceEventTarget) != null)
                {
                    eventTarget.OnIconChanged(icon);
                }
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetActive()
        {
            SetActiveWindow(handle);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetFocus()
        {
            SetFocus(handle);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetBounds(int x, int y, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                SetWindowPos(handle, IntPtr.Zero, x, y, Math.Max(0, width), Math.Max(0, height), SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
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
        public void SetIcon(Icon value)
        {
            if (handle != IntPtr.Zero)
            {
                if (value != null)
                {
                    SendMessage(handle, WindowMessage.WM_SETICON, SetIconParameter.ICON_SMALL.ToHandle(), value.Handle);
                    SendMessage(handle, WindowMessage.WM_SETICON, SetIconParameter.ICON_BIG.ToHandle(), value.Handle);
                }
                else
                {
                    SendMessage(handle, WindowMessage.WM_SETICON, SetIconParameter.ICON_SMALL.ToHandle(), IntPtr.Zero);
                    SendMessage(handle, WindowMessage.WM_SETICON, SetIconParameter.ICON_BIG.ToHandle(), IntPtr.Zero);
                }
            }
            else icon = value;
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
                        SetWindowLongPtr(handle, WindowLongIndexFlags.GWLP_HWNDPARENT, MessageWindow.Default.Handle);
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
                ShowWindowCommand cmd; switch (value)
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
                ShowWindowCommand com; if (value)
                {
                    if ((appearance & Appearance.Passive) == Appearance.Passive)
                    {
                        com = ShowWindowCommand.SW_SHOWNOACTIVATE;
                    }
                    else com = ShowWindowCommand.SW_SHOW;
                }
                else com = ShowWindowCommand.SW_HIDE;
                ShowWindow(handle, com);
            }
            else visible = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetOrder(bool top)
        {
            if (handle != IntPtr.Zero)
            {
                SetWindowPosFlags flags = SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE;
                if ((appearance & Appearance.Passive) == Appearance.Passive)
                {
                    flags |= SetWindowPosFlags.SWP_NOACTIVATE;
                }
                SetWindowPos(handle, (top ? WindowOrder.HWND_TOP : WindowOrder.HWND_BOTTOM).ToHandle(), 0, 0, 0, 0, flags);
            }
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
        public void Invalidate()
        {
            dirty = true;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Redraw([Implicit(true)] IWindow host)
        {
            if (dirty)
            {
                dirty = false;
                IRendererEventTarget eventTarget; if((eventTarget = host as IRendererEventTarget) != null)
                {
                    eventTarget.OnFlushBuffer();
                }
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