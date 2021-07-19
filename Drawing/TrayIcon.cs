// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Runtime.CompilerServices;
using SE.Hyperion.Desktop;
using SE.Mixin;

namespace SE.Hyperion.Drawing
{
    public abstract partial class TrayIcon : FinalizerObject
    {
        public abstract IntPtr Handle
        {
            get;
        }

        public int Id
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return unchecked((long)Handle).GetHashCode(); }
        }

        public abstract Icon Icon
        {
            get;
            set;
        }
        public abstract string Tooltip
        {
            get;
            set;
        }
        public abstract bool Visible
        {
            get;
            set;
        }

        private static void CreateType()
        {
            #if DEBUG
            try
            {
            #endif
                if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
                {
                    //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                    Compositor.DeclareType<TrayIcon>
                    (
                        typeof(Desktop.Win32.TrayIcon),
                        typeof(TrayListener)
                    );
                }
                else Compositor.DeclareType<TrayIcon>(typeof(Desktop.X11.TrayIcon));
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
        public TrayIcon()
        { }
        protected override void Dispose(bool disposing)
        {
            if(!Disposed)
            {
                ITrayEventTarget tt; if ((tt = this as ITrayEventTarget) != null)
                {
                    tt.OnVisibleChanged(false);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public abstract void SetOwner(ITrayContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static TrayIcon Create()
        {
            return Compositor.CreateInstance<TrayIcon>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static TrayIcon Create(ITrayContext context)
        {
            TrayIcon icon = Compositor.CreateInstance<TrayIcon>();
            icon.SetOwner(context);

            return icon;
        }
    }
}
