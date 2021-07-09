// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Runtime.CompilerServices;
using SE.Hyperion.Desktop;
using SE.Mixin;
using SE.Reactive;

using Notifier = SE.Reactive.ReactiveNotifier<System.Drawing.Point, SE.Reactive.ReactiveStream<SE.Reactive.NotifyArgs>>;

namespace SE.Hyperion.Drawing
{
    public abstract class TrayIcon : FinalizerObject
    {
        internal struct IconPropertyMeta : IReactiveProperty<Icon>
        {
            private readonly PropertyId id;
            public PropertyId Id
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return id.Property; }
            }
            public Icon Default
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return default(Icon); }
            }

            public IconPropertyMeta(UInt32 id)
            {
                this.id = new PropertyId(id);
            }
            
            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Set(object instance, Icon value)
            {
                TrayIcon inst = (instance as TrayIcon);
                lock (inst)
                {
                    bool result = inst.iconFlag;
                    inst.iconFlag = true;
                    inst.SetIcon(value);

                    return result;
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool TryGet(object instance, out Icon value)
            {
                TrayIcon inst = (instance as TrayIcon);
                lock (inst)
                {
                    if (inst.iconFlag)
                    {
                        value = inst.Icon;
                        return true;
                    }
                    else
                    {
                        value = Default;
                        return false;
                    }
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Clear(object instance)
            {
                TrayIcon inst = (instance as TrayIcon);
                lock (inst)
                {
                    bool result = inst.iconFlag;
                    inst.iconFlag = false;
                    inst.SetIcon(Default);

                    return result;
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public IDisposable Subscribe(IObserver<PropertyId> observer)
            {
                return PropertyStream<Icon, ReactiveStream<PropertyId>>.Subscribe(id, observer);
            }
        }

        public readonly static IReactiveProperty<Icon> IconProperty;

        public readonly static IReactiveNotifier<Point> ClickEvent;
        public readonly static IReactiveNotifier<Point> DoubleClickEvent;
        public readonly static IReactiveNotifier<Point> RightClickEvent;

        IPlatformObject owner;

        public abstract IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return unchecked((long)Handle).GetHashCode(); }
        }

        bool iconFlag;
        /// <summary>
        /// 
        /// </summary>
        public virtual Icon Icon
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { IconProperty.Set(this, value); }
        }

        //bool tooltipFlag;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Tooltip
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetTooltip(value); }
        }

        //bool visibleFlag;
        /// <summary>
        /// 
        /// </summary>
        public virtual bool Visible
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetVisible(value); }
        }

        static TrayIcon()
        {
            #if DEBUG
            try
            {
            #endif
                if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
                {
                    //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                    Compositor.DeclareType<TrayIcon>(typeof(Desktop.Win32.TrayIcon));
                }
                else Compositor.DeclareType<TrayIcon>(typeof(Desktop.X11.TrayIcon));
            #if DEBUG
            }
            catch(Exception er)
            {
                throw er;
            }
            #endif
            IconProperty = new IconPropertyMeta((UInt32)UniqueId.Next32());
            ClickEvent = new Notifier((UInt32)UniqueId.Next32());
            DoubleClickEvent = new Notifier((UInt32)UniqueId.Next32());
            RightClickEvent = new Notifier((UInt32)UniqueId.Next32());
        }
        /// <summary>
        /// 
        /// </summary>
        public TrayIcon(IPlatformObject owner)
        {
            this.owner = owner;
        }
        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                OnVisibleChanged(false);
            }
            base.Dispose(disposing);
        }

        protected virtual void OnIconChanged()
        {
            PropertyStream<Icon, ReactiveStream<PropertyId>>.Push(IconProperty.Id | this);
        }
        protected abstract void SetIcon(Icon icon);

        protected virtual void OnTooltipChanged()
        {
            //PropertyStream<Icon>.Push(TooltipProperty.Id | this);
        }
        protected abstract void SetTooltip(string tooltip);

        protected virtual void OnVisibleChanged(bool visible)
        {
            //PropertyStream<Icon>.Push(VisibleProperty.Id | this);
        }
        protected abstract void SetVisible(bool visible);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static TrayIcon Create()
        {
            return Compositor.CreateInstance<TrayIcon>(new object[1]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static TrayIcon Create(IPlatformObject owner)
        {
            return Compositor.CreateInstance<TrayIcon>(owner);
        }
    }
}
