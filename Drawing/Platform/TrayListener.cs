// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using SE.Hyperion.Desktop;
using SE.Mixin;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    public struct TrayListener
    {
        [Multicast]
        public static void OnIconChanged([Implicit(true)] ITrayEventTarget host, Icon icon)
        {
            TrayIcon tmp; if ((tmp = host as TrayIcon) != null)
            {
                PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(TrayIcon.IconProperty.Id | tmp.Id);
            }
        }

        [Multicast]
        public static void OnTooltipChanged([Implicit(true)] ITrayEventTarget host, string title)
        {
            TrayIcon tmp; if ((tmp = host as TrayIcon) != null)
            {
                PropertyStream<string, ReactiveStream<PropertyId>>.Push(TrayIcon.TooltipProperty.Id | tmp.Id);
            }
        }

        [Multicast]
        public static void OnVisibleChanged([Implicit(true)] ITrayEventTarget host, bool visible)
        {
            TrayIcon tmp; if ((tmp = host as TrayIcon) != null)
            {
                PropertyStream<bool, ReactiveStream<PropertyId>>.Push(TrayIcon.VisibleProperty.Id | tmp.Id);
            }
        }
    }
}
