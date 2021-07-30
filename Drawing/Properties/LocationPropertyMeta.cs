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
    public class LocationPropertyMeta : IReactiveProperty<Point>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Point> Instance;

        private readonly PropertyId id;
        public PropertyId Id
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return id.Property; }
        }
        public Point Default
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return Point.Empty; }
        }

        static LocationPropertyMeta()
        {
            Instance = new LocationPropertyMeta();
        }
        private LocationPropertyMeta()
        {
            this.id = new PropertyId((UInt32)UniqueId.Next32());
        }
            
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Set(object instance, Point value)
        {
            Surface tmp; if ((tmp = instance as Surface) != null)
            {
                tmp.Location = value;
                return true;
            }
            else return PropertyStream<Point, ReactiveStream<PropertyId>>.Set(instance, id, ref value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool TryGet(object instance, out Point value)
        {
            Surface tmp; if ((tmp = instance as Surface) != null)
            {
                value = tmp.Location;
                return true;
            }
            else return PropertyStream<Point, ReactiveStream<PropertyId>>.TryGet(instance, id, out value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            Surface tmp; if ((tmp = instance as Surface) != null)
            {
                tmp.Location = Default;
                return true;
            }
            else return PropertyStream<Point, ReactiveStream<PropertyId>>.Clear(instance, id);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
        {
            return PropertyStream<Point, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
        }
    }
}
