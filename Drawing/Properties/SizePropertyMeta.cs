// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SE.Reactive;

namespace SE.Hyperion.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public class SizePropertyMeta : IReactiveProperty<Size>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Size> Instance;

        private readonly PropertyId id;
        public PropertyId Id
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return id.Property; }
        }
        public Size Default
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return Size.Empty; }
        }

        static SizePropertyMeta()
        {
            Instance = new SizePropertyMeta();
        }
        private SizePropertyMeta()
        {
            this.id = new PropertyId((UInt32)UniqueId.Next32());
        }
            
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Set(object instance, Size value)
        {
            Surface tmp; if ((tmp = instance as Surface) == null)
            {
                tmp.Size = value;
                return true;
            }
            else return PropertyStream<Size, ReactiveStream<PropertyId>>.Set(instance, id, ref value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool TryGet(object instance, out Size value)
        {
            Surface tmp; if ((tmp = instance as Surface) == null)
            {
                value = tmp.Size;
                return true;
            }
            else return PropertyStream<Size, ReactiveStream<PropertyId>>.TryGet(instance, id, out value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            Surface tmp; if ((tmp = instance as Surface) == null)
            {
                tmp.Size = Default;
                return true;
            }
            else return PropertyStream<Size, ReactiveStream<PropertyId>>.Clear(instance, id);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(IObserver<PropertyId> observer)
        {
            return PropertyStream<Size, ReactiveStream<PropertyId>>.Subscribe(id, observer);
        }
    }
}
