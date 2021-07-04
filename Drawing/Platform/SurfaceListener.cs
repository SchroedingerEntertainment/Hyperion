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
    public struct SurfaceListener
    {
        [Multicast]
        public static void OnCreated([Implicit(true)] ISurfaceEventTarget host)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<IntPtr, ReactiveStream<PropertyId>>.Push(Surface.HandleProperty.Id | tmp);
                Surface.CreateEvent.Invoke(tmp);
            }
        }

        [Multicast]
        public static void OnMove([Implicit(true)] ISurfaceEventTarget host, Point location)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Point, ReactiveStream<PropertyId>>.Push(Surface.LocationProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnResize([Implicit(true)] ISurfaceEventTarget host, Size size)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<Size, ReactiveStream<PropertyId>>.Push(Surface.SizeProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnVisibleChanged([Implicit(true)] ISurfaceEventTarget host, bool visible)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                PropertyStream<bool, ReactiveStream<PropertyId>>.Push(Surface.VisibleProperty.Id | tmp);
            }
        }

        [Multicast]
        public static void OnClose([Implicit(true)] ISurfaceEventTarget host)
        {
            Surface tmp; if ((tmp = host as Surface) != null)
            {
                Surface.CloseEvent.Invoke(tmp);
            }
        }
    }
}
