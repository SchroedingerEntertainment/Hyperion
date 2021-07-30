// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public struct HandlePropertyMeta : IReactiveProperty<IntPtr>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<IntPtr> Instance;

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
            Surface sf = (instance as Surface);
            if (sf != null && (value = sf.Handle) != IntPtr.Zero)
                return true;

            TrayIcon ti = (instance as TrayIcon);
            if (ti != null && (value = ti.Handle) != IntPtr.Zero)
                return true;

            value = IntPtr.Zero;
            return false;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            throw new MemberAccessException();
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
        {
            return PropertyStream<IntPtr, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
        }
    }
}
