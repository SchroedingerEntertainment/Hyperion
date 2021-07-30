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
    public partial struct SurfaceListener
    {
        [Multicast]
        public static void OnIconChanged([Implicit(true)] IAppearanceEventTarget host, Icon icon)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(Surface.VisibleProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnTransparencyChanged([Implicit(true)] IAppearanceEventTarget host, Transparency transparency)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(Surface.VisibleProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnTitleChanged([Implicit(true)] IAppearanceEventTarget host, string title)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(Surface.VisibleProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnStateChanged([Implicit(true)] IAppearanceEventTarget host, WindowState state)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(Surface.VisibleProperty.Id | tmp);
            }
        }
    }
}
