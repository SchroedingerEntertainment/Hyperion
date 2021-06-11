// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Desktop.Win32
{
    public struct TrayIcon : IDisposable
    {
        IntPtr handle;
        Guid guid;

        [Access(AccessFlag.Set)]
        Icon icon;

        [Access(AccessFlag.Set)]
        string tooltip;

        [Access(AccessFlag.Set)]
        bool visible;

        public TrayIcon([Generator(GeneratorFlag.Implicit)] ISurface host)
        {
            this.handle = host.Handle;
            this.guid = Guid.NewGuid();
            this.icon = null;
            this.tooltip = string.Empty;
            this.visible = false;
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                
                handle = IntPtr.Zero;
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Initialize()
        {
            if (handle == IntPtr.Zero)
            {
                return CreateInstance(host);
            }
            else return false;
        }
        bool CreateInstance(ISurface host)
        {
            guid = Guid.NewGuid();

            NotifyIconData data = NotifyIconData.Create(host.Handle, guid);
            data.uFlags |= NotifyFlags.NIF_ICON | NotifyFlags.NIF_TIP | NotifyFlags.NIF_MESSAGE;
            data.uCallbackMessage = (WindowMessage.WM_APP + 1);

            if (NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_ADD, ref data))
            {
                handle = host.Handle;

                data = NotifyIconData.Create(handle, guid);
                return NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_DELETE, ref data);
            }
            else return false;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetIcon(Icon value)
        {
            if (handle != IntPtr.Zero)
            {
                NotifyIconData data = NotifyIconData.Create(handle, guid);
                data.uFlags |= NotifyFlags.NIF_ICON;
                data.hIcon = value.Handle;

                NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_MODIFY, ref data);
                icon = value;
            }
            else icon = value;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetVisible(bool value)
        {
            if (handle != IntPtr.Zero)
            {
                if (value)
                {
                    NotifyIconData data = NotifyIconData.Create(handle, guid);
                    data.uFlags |= NotifyFlags.NIF_ICON | NotifyFlags.NIF_TIP | NotifyFlags.NIF_MESSAGE;
                    data.uCallbackMessage = (WindowMessage.WM_APP + 1);

                    visible = NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_ADD, ref data);
                }
                else
                {
                    NotifyIconData data = NotifyIconData.Create(handle, guid);
                    visible = !NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_DELETE, ref data);
                }
            }
            else visible = value;
        }
    }
}
