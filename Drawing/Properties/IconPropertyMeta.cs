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
    public class IconPropertyMeta : IReactiveProperty<Icon>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static IReactiveProperty<Icon> Instance;

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

        static IconPropertyMeta()
        {
            Instance = new IconPropertyMeta();
        }
        private IconPropertyMeta()
        {
            this.id = new PropertyId((UInt32)UniqueId.Next32());
        }
            
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Set(object instance, Icon value)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                sf.Icon = value;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    ti.Icon = value;
                    return true;
                }
                else return PropertyStream<Icon, ReactiveStream<PropertyId>>.Set(instance, id, ref value);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool TryGet(object instance, out Icon value)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                value = sf.Icon;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    value = ti.Icon;
                    return true;
                }
                else return PropertyStream<Icon, ReactiveStream<PropertyId>>.TryGet(instance, id, out value);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            Surface sf; if ((sf = instance as Surface) != null)
            {
                sf.Icon = Default;
                return true;
            }
            else
            {
                TrayIcon ti; if ((ti = instance as TrayIcon) != null)
                {
                    ti.Icon = Default;
                    return true;
                }
                else return PropertyStream<Icon, ReactiveStream<PropertyId>>.Clear(instance, id);
            }
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(IObserver<PropertyId> observer)
        {
            return PropertyStream<Icon, ReactiveStream<PropertyId>>.Subscribe(id, observer);
        }
    }
}
