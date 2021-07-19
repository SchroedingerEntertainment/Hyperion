// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    public partial class TrayIcon
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<IntPtr> HandleProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Icon> IconProperty;
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<string> TooltipProperty;
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<bool> VisibleProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveNotifier<MouseEventArgs> MouseEvent;
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveNotifier RefreshEvent;

        static TrayIcon()
        {
            HandleProperty = new HandlePropertyMeta((UInt32)UniqueId.Next32());

            IconProperty = IconPropertyMeta.Instance;
            TooltipProperty = TooltipPropertyMeta.Instance;
            VisibleProperty = VisiblePropertyMeta.Instance;

            MouseEvent = new ReactiveNotifier<MouseEventArgs, ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());
            RefreshEvent = new ReactiveNotifier<ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());

            CreateType();
        }
    }
}
