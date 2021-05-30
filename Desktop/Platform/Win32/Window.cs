// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
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
            if (atom == 0)
            {
                WindowClassEx cls = WindowClassEx.Create();
                cls.style = ClassStyles.CS_VREDRAW | ClassStyles.CS_HREDRAW;
                cls.lpfnWndProc = wndProc;
                cls.hInstance = GetModuleHandle(null);
                cls.hCursor = Cursor.Default;
                cls.lpszClassName = Guid.NewGuid().ToString();
                
                atom = RegisterClassEx(ref cls);
            }
            if (atom != 0)
            {
                handle = CreateWindowEx(0, atom, null, WindowStyles.WS_OVERLAPPEDWINDOW, 0, 0, 300, 300, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                return (handle != IntPtr.Zero);
            }
            else return false;
        }

        IntPtr WndProc(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_ENTERSIZEMOVE:
                    {
                        sizeMoveFlag = true;
                    }
                    break;
                case WindowMessage.WM_EXITSIZEMOVE:
                    {
                        sizeMoveFlag = false;
                    }
                    break;
                case WindowMessage.WM_PAINT:
                case WindowMessage.WM_SYNCPAINT:
                    {
                        if (!sizeMoveFlag)
                        {
                            dirtyFlag.Exchange(true);
                        }
                        else OnFlushBuffer();
                    }
                    break;
                case WindowMessage.WM_DESTROY:
                    {
                        handle = IntPtr.Zero;
                    }
                    break;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
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

        public override TransparencyMask SetTransparencyMask(TransparencyMask mask)
        {
            if (Shared.OsVersion.Major >= 6)
            {
                bool enabled; if (DwmIsCompositionEnabled(out enabled) != 0 || !enabled)
                {
                    return TransparencyMask.None;
                }
                else if (Shared.OsVersion.Major >= 10)
                {
                    return SetTransparencyMask_Gradient(mask);
                }
                else if (Shared.OsVersion.Minor >= 2)
                {
                    return SetTransparencyMask_Composition(mask);
                }
                else return SetTransparencyMask_DWM(mask);
            }
            else return TransparencyMask.None;
        }
        TransparencyMask SetTransparencyMask_Gradient(TransparencyMask mask)
        {
            AccentPolicy policy = new AccentPolicy();

            switch (mask)
            {
                case TransparencyMask.AcrylicBlur:
                    {
                        if (Shared.OsVersion.Major <= 10 && Shared.OsVersion.Build < 19628)
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

            SetWindowCompositionAttribute(handle, ref data);

            Marshal.FreeHGlobal(policyPtr);
            return mask;
        }
        TransparencyMask SetTransparencyMask_Composition(TransparencyMask mask)
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

            SetWindowCompositionAttribute(handle, ref data);
            Marshal.FreeHGlobal(policyPtr);

            if (mask == TransparencyMask.Blur)
            {
                SetTransparencyMask_DWM(mask);
            }
            return mask;
        }
        TransparencyMask SetTransparencyMask_DWM(TransparencyMask mask)
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
                        DwmEnableBlurBehindWindow(handle, ref nfo);
                    }
                    return mask;
            }
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