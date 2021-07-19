// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using SE.Mixin;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct PopupComponent : IDisposable
    {
        private readonly static int HeaderSize;
        private readonly static int InputSize;

        private Platform.WinEventProcPtr winEventProc;
        private IntPtr eventHook;

        static PopupComponent()
        {
            HeaderSize = Marshal.SizeOf(typeof(RawInputHeader));
            InputSize = Marshal.SizeOf(typeof(RawInput));
        }
        public PopupComponent([Implicit(true)] INative host)
        {
            this.winEventProc = (hWinEventHook, @event, hwnd, idObject, idChild, dwEventThread, dwmsEventTime) => Window.PostMessage(host.Handle, WindowMessage.WM_UNINITMENUPOPUP, IntPtr.Zero, IntPtr.Zero);
            this.eventHook = IntPtr.Zero;
        }
        public void Dispose()
        {
            if (eventHook != IntPtr.Zero)
            {
                UnhookInput();
                if (Platform.UnhookWinEvent(eventHook))
                    eventHook = IntPtr.Zero;
            }
        }

        [WndProc(WindowMessage.WM_UNINITMENUPOPUP)]
        [WndProc(WindowMessage.WM_MOUSEACTIVATE)]
        [WndProc(WindowMessage.WM_ACTIVATE)]
        [WndProc(WindowMessage.WM_SETFOCUS)]
        [WndProc(WindowMessage.WM_INPUT)]
        public IntPtr WndProc(IWindow host, IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.WM_MOUSEACTIVATE: return MouseActive.MA_NOACTIVATE.ToHandle();
                case WindowMessage.WM_UNINITMENUPOPUP:
                    {
                        IPopupEventTarget eventTarget; if ((eventTarget = host as IPopupEventTarget) != null)
                        {
                            eventTarget.OnPopupClose();
                        }
                    }
                    break;
                case WindowMessage.WM_ACTIVATE: switch ((ActivationMode)wParam.LoWord())
                    {
                        case ActivationMode.WA_ACTIVE:
                        case ActivationMode.WA_CLICKACTIVE:
                            {
                                Window.SetActiveWindow(lParam);
                            }
                            break;
                        default: return Window.DefWindowProc(hwnd, msg, wParam, lParam);
                    }
                    break;
                case WindowMessage.WM_SETFOCUS:
                    {
                        Window.SetFocus(wParam);
                    }
                    break;
                case WindowMessage.WM_INPUT:
                    {
                        OnInput(host, lParam);
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        [Multicast]
        public void OnVisibleChanged([Implicit(true)] IPopupWindow host, bool visible)
        {
            if (host.Handle != IntPtr.Zero)
            {
                if (visible && eventHook == IntPtr.Zero)
                {
                    RegisterEventHook();
                    RegisterInputHook(host.Handle);
                }
                else if (eventHook != IntPtr.Zero)
                {
                    UnhookInput();
                    if (Platform.UnhookWinEvent(eventHook))
                        eventHook = IntPtr.Zero;
                }
            }
            else if (eventHook != IntPtr.Zero)
            {
                UnhookInput();
                if (Platform.UnhookWinEvent(eventHook))
                    eventHook = IntPtr.Zero;
            }
        }
        private void OnInput(IWindow host, IntPtr data)
        {
            if (host.Handle != IntPtr.Zero)
            {
                int size = InputSize;
                RawInput header;

                if (Platform.GetRawInputData(data, RawData.RID_INPUT, out header, ref size, HeaderSize) > 0 && header.Header.Type == RawInputType.Mouse)
                {
                    switch (header.Data.Mouse.uButtons.Data.usButtonFlags)
                    {
                        case RawMouseButton.RI_MOUSE_LEFT_BUTTON_DOWN:
                        case RawMouseButton.RI_MOUSE_RIGHT_BUTTON_DOWN:
                            {
                                Point pt;
                                GetCursorPos(out pt);

                                if (!host.Bounds.Contains(pt.ToPoint()))
                                    Window.PostMessage(host.Handle, WindowMessage.WM_UNINITMENUPOPUP, IntPtr.Zero, IntPtr.Zero);
                            }
                            break;
                    }
                }
            }
        }

        void RegisterEventHook()
        {
            eventHook = Platform.SetWinEventHook(WinEventHook.EVENT_SYSTEM_FOREGROUND, WinEventHook.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, winEventProc, 0, 0, WinEventHookFlags.WINEVENT_OUTOFCONTEXT);
            if (eventHook == IntPtr.Zero)
                throw Platform.GetLastWin32Error();
        }
        void RegisterInputHook(IntPtr handle)
        {
            RAWINPUTDEVICE[] devices = new RAWINPUTDEVICE[1];

            devices[0].WindowHandle = handle;
            devices[0].UsagePage = HidUsagePage.Generic;
            devices[0].Usage = HidUsage.Mouse;
            devices[0].Flags = RawInputDeviceFlags.InputSink;

            if (!Platform.RegisterRawInputDevices(devices, 1, Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
                throw Platform.GetLastWin32Error();
        }

        void UnhookInput()
        {
            RAWINPUTDEVICE[] devices = new RAWINPUTDEVICE[1];

            devices[0].UsagePage = HidUsagePage.Generic;
            devices[0].Usage = HidUsage.Mouse;
            devices[0].Flags = RawInputDeviceFlags.Remove;

            if (!Platform.RegisterRawInputDevices(devices, 1, Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
                throw Platform.GetLastWin32Error();
        }
    }
}
