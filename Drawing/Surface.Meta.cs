// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    public partial class Surface
    {
        internal struct HandlePropertyMeta : IReactiveProperty<IntPtr>
        {
            private readonly PropertyId id;
            public PropertyId Id
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return id.Property; }
            }
            public IntPtr Default
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return IntPtr.Zero; }
            }

            public HandlePropertyMeta(UInt32 id)
            {
                this.id = new PropertyId(id);
            }
            
            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Set(object instance, IntPtr value)
            {
                throw new MemberAccessException();
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool TryGet(object instance, out IntPtr value)
            {
                Surface tmp = (instance as Surface);
                if (tmp == null || (value = tmp.Handle) == IntPtr.Zero)
                {
                    value = IntPtr.Zero;
                    return false;
                }
                else return true;
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public bool Clear(object instance)
            {
                throw new MemberAccessException();
            }

            [MethodImpl(OptimizationExtensions.ForceInline)]
            public IDisposable Subscribe(IObserver<PropertyId> observer)
            {
                return PropertyStream<IntPtr, ReactiveStream<PropertyId>>.Subscribe(id, observer);
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

            VisibleProperty = VisiblePropertyMeta.Instance;

            CreateEvent = new ReactiveNotifier<ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());
            CloseEvent = new ReactiveNotifier<ReactiveStream<NotifyArgs>>((UInt32)UniqueId.Next32());

            CreateType();
        }
    }
}
