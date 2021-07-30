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
    public class TooltipPropertyMeta : IReactiveProperty<string>
    {
        /// <summary>
        /// 
        /// </summary>
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

        static TooltipPropertyMeta()
        {
            Instance = new TooltipPropertyMeta();
        }
        private TooltipPropertyMeta()
        {
            this.id = new PropertyId((UInt32)UniqueId.Next32());
        }
            
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Set(object instance, string value)
        {
            TrayIcon ti; if ((ti = instance as TrayIcon) != null)
            {
                ti.Tooltip = value;
                return true;
            }
            else return PropertyStream<string, ReactiveStream<PropertyId>>.Set(instance, id, ref value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool TryGet(object instance, out string value)
        {
            TrayIcon ti; if ((ti = instance as TrayIcon) != null)
            {
                value = ti.Tooltip;
                return true;
            }
            else return PropertyStream<string, ReactiveStream<PropertyId>>.TryGet(instance, id, out value);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool Clear(object instance)
        {
            TrayIcon ti; if ((ti = instance as TrayIcon) != null)
            {
                ti.Tooltip = Default;
                return true;
            }
            else return PropertyStream<string, ReactiveStream<PropertyId>>.Clear(instance, id);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public IDisposable Subscribe(object owner, IObserver<PropertyId> observer)
        {
            return PropertyStream<string, ReactiveStream<PropertyId>>.Subscribe(id, owner, observer);
        }
    }
}
