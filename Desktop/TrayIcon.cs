// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Runtime.CompilerServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop
{
    public abstract class TrayIcon : FinalizerObject, IPlatformObject, ITrayIcon
    {
        IPlatformObject owner;

        public abstract IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Icon Icon
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetIcon(value); }
        }

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
                    Compositor.DeclareType<TrayIcon>(typeof(Win32.TrayIcon));
                }
                else Compositor.DeclareType<TrayIcon>(typeof(X11.TrayIcon));
            #if DEBUG
            }
            catch(Exception er)
            {
                throw er;
            }
            #endif
        }
        /// <summary>
        /// 
        /// </summary>
        public TrayIcon(IPlatformObject owner)
        {
            this.owner = owner;
        }

        public abstract bool Initialize(IPlatformObject owner);

        public abstract void SetIcon(Icon icon);
        public abstract void SetTooltip(string tooltip);
        public abstract void SetVisible(bool visible);

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
