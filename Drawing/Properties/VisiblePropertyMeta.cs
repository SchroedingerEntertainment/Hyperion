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
    public class VisiblePropertyMeta : IReactiveProperty<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<bool> Instance;

        private readonly PropertyId id;
        public PropertyId Id
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return id.Property; }
        }
        public bool Default
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return default(bool); }
        }

        static VisiblePropertyMeta()
        {
            Instance = new VisiblePropertyMeta();
        }
        private VisiblePropertyMeta()
        {
            this.id = new PropertyId((UInt32)UniqueId.Next32());
        }
            
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Set(object instance, bool value)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                sf.Visible = value;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    ti.Visible = value;
                    return true;
                }
                else return PropertyStream<bool, ReactiveStream<PropertyId>>.Set(instance, id, ref value);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool TryGet(object instance, out bool value)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                value = sf.Visible;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    value = ti.Visible;
                    return true;
                }
                else return PropertyStream<bool, ReactiveStream<PropertyId>>.TryGet(instance, id, out value);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                sf.Visible = Default;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    ti.Visible = Default;
                    return true;
                }
                else return PropertyStream<bool, ReactiveStream<PropertyId>>.Clear(instance, id);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
        {
            return PropertyStream<bool, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
        }
    }
}
