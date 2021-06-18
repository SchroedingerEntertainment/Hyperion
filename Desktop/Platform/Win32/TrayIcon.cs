// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop.Win32
{
    public struct TrayIcon : IDisposable
    {
        Guid guid;

        [Access(AccessFlag.Get)]
        public IntPtr handle;

        [Access(AccessFlag.Get)]
        public Icon icon;

        [Access(AccessFlag.Get)]
        public string tooltip;

        [Access(AccessFlag.Get)]
        public bool visible;

        public TrayIcon([Generator(GeneratorFlag.Default)] IPlatformObject host)
        {
            this.handle = (host != null) ? host.Handle : Platform.MessageReceiver;
            this.tooltip = string.Empty;
            this.guid = Guid.NewGuid();
            this.visible = false;
            this.icon = null;
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                SetVisible(null, false);
                handle = IntPtr.Zero;
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetIcon([Generator(GeneratorFlag.Implicit)] ITrayIconEvents eventHandler, Icon value)
        {
            if (handle != IntPtr.Zero && visible)
            {
                NotifyIconData data = NotifyIconData.Create(handle, guid);
                data.uFlags |= NotifyFlags.NIF_ICON;
                if (!string.IsNullOrWhiteSpace(tooltip))
                {
                    data.uFlags |= NotifyFlags.NIF_SHOWTIP;
                }
                data.hIcon = value.Handle;

                NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_MODIFY, ref data);
            }
            icon = value;
            eventHandler.OnIconChanged();
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetTooltip([Generator(GeneratorFlag.Implicit)] ITrayIconEvents eventHandler, string value)
        {
            if (handle != IntPtr.Zero && visible)
            {
                NotifyIconData data = NotifyIconData.Create(handle, guid);
                data.uFlags |= NotifyFlags.NIF_TIP;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    data.uFlags |= NotifyFlags.NIF_SHOWTIP;
                    data.szTip = value;
                }
                NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_MODIFY, ref data);
            }
            tooltip = value;
            eventHandler.OnTooltipChanged();
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void SetVisible([Generator(GeneratorFlag.Implicit)] ITrayIconEvents eventHandler, bool value)
        {
            if (value == visible)
            {
                return;
            }
            else if (handle != IntPtr.Zero)
            {
                if (value)
                {
                    NotifyIconData data = NotifyIconData.Create(handle, guid);
                    data.uFlags |= NotifyFlags.NIF_MESSAGE;
                    data.uCallbackMessage = WindowMessage.WM_NOTIFY_EVENT;
                    data.uTimeoutOrVersion = 0x4;
                    if (icon != null)
                    {
                        data.uFlags |= NotifyFlags.NIF_ICON;
                        data.hIcon = icon.Handle;
                    }
                    if (!string.IsNullOrWhiteSpace(tooltip))
                    {
                        data.uFlags |= NotifyFlags.NIF_TIP | NotifyFlags.NIF_SHOWTIP;
                        data.szTip = tooltip;
                    }
                    visible = NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_ADD, ref data) &&
                              NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_SETVERSION, ref data);
                }
                else
                {
                    NotifyIconData data = NotifyIconData.Create(handle, guid);
                    visible = !NotifyIcon.Shell_NotifyIcon(NotifyMessage.NIM_DELETE, ref data);
                }
            }
            else visible = false;
            if (eventHandler != null)
            {
                eventHandler.OnVisibleChanged(visible);
            }
        }
    }
}
