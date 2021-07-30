// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Hyperion.Desktop;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    public partial class Surface
    {
        class TransparencyPropertyMeta : IReactiveProperty<Transparency>
        {
            public readonly static IReactiveProperty<Transparency> Instance;

            private readonly PropertyId id;
            public PropertyId Id
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return id.Property; }
            }
            public Transparency Default
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return Transparency.None; }
            }

            static TransparencyPropertyMeta()
            {
                Instance = new TransparencyPropertyMeta();
            }
            private TransparencyPropertyMeta()
            {
                this.id = new PropertyId((UInt32)UniqueId.Next32());
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Set(object instance, Transparency value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.Transparency = value;
                    return true;
                }
                else throw new InvalidOperationException();
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool TryGet(object instance, out Transparency value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    value = tmp.Transparency;
                    return true;
                }
                else
                {
                    value = Default;
                    return false;
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Clear(object instance)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.Transparency = Default;
                    return true;
                }
                else return false;
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
            {
                return PropertyStream<Transparency, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
            }
        }
        class StatePropertyMeta : IReactiveProperty<WindowState>
        {
            public readonly static IReactiveProperty<WindowState> Instance;

            private readonly PropertyId id;
            public PropertyId Id
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return id.Property; }
            }
            public WindowState Default
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return WindowState.Normal; }
            }

            static StatePropertyMeta()
            {
                Instance = new StatePropertyMeta();
            }
            private StatePropertyMeta()
            {
                this.id = new PropertyId((UInt32)UniqueId.Next32());
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Set(object instance, WindowState value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.State = value;
                    return true;
                }
                else throw new InvalidOperationException();
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool TryGet(object instance, out WindowState value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    value = tmp.State;
                    return true;
                }
                else
                {
                    value = Default;
                    return false;
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Clear(object instance)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.State = Default;
                    return true;
                }
                else return false;
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
            {
                return PropertyStream<WindowState, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
            }
        }
        class TitlePropertyMeta : IReactiveProperty<string>
        {
            public readonly static IReactiveProperty<string> Instance;

            private readonly PropertyId id;
            public PropertyId Id
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return id.Property; }
            }
            public string Default
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return string.Empty; }
            }

            static TitlePropertyMeta()
            {
                Instance = new TitlePropertyMeta();
            }
            private TitlePropertyMeta()
            {
                this.id = new PropertyId((UInt32)UniqueId.Next32());
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Set(object instance, string value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.Title = value;
                    return true;
                }
                else throw new InvalidOperationException();
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool TryGet(object instance, out string value)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    value = tmp.Title;
                    return true;
                }
                else
                {
                    value = Default;
                    return false;
                }
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Clear(object instance)
            {
                Surface tmp; if ((tmp = instance as Surface) != null)
                {
                    tmp.Title = Default;
                    return true;
                }
                else return false;
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
            {
                return PropertyStream<string, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<IntPtr> HandleProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Point> LocationProperty;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Size> SizeProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Transparency> TransparencyProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<WindowState> StateProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<string> TitleProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<bool> VisibleProperty;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveNotifier CreateEvent;

        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveNotifier CloseEvent;

        static Surface()
        {
            HandleProperty = new HandlePropertyMeta((UInt32)UniqueId.Next32());
            LocationProperty = LocationPropertyMeta.Instance;
            SizeProperty = SizePropertyMeta.Instance;
            TransparencyProperty = TransparencyPropertyMeta.Instance;
            StateProperty = StatePropertyMeta.Instance;
            TitleProperty = TitlePropertyMeta.Instance;
            VisibleProperty = VisiblePropertyMeta.Instance;

            CreateEvent = new ReactiveNotifier<ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());
            CloseEvent = new ReactiveNotifier<ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());

            CreateType();
        }
    }
}
