// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Runtime.CompilerServices;
using SE.Hyperion.Desktop;
using SE.Mixin;

[assembly: InternalsVisibleTo("Mixin")]

namespace SE.Hyperion.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class SurfaceManager
    {
        internal abstract class SurfaceManagerInternal : ITrayActionEventTarget
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            public virtual bool OnMouse(long id, MouseButton button, Point cursor)
            {
                switch (button)
                {
                    case MouseButton.Left:
                    case MouseButton.Double:
                    case MouseButton.Right:
                        {
                            TrayIcon.MouseEvent.Invoke(id, new MouseEventArgs(button, cursor));
                        }
                        break;
                }
                return true;
            }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            public virtual bool OnRefresh(long id)
            {
                TrayIcon.RefreshEvent.Invoke(id);
                return true;
            }

            internal abstract bool ProcessEvent();
        }
        private readonly static SurfaceManagerInternal api;

        static SurfaceManager()
        {
            #if DEBUG
            try
            {
            #endif
                if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
                {
                    //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                    Compositor.DeclareType<SurfaceManagerInternal>(typeof(Desktop.Win32.Platform));
                }
                else Compositor.DeclareType<SurfaceManagerInternal>(typeof(Desktop.X11.Platform));
                api = Compositor.CreateInstance<SurfaceManagerInternal>();
            #if DEBUG
            }
            catch (Exception er)
            {
                throw er;
            }
            #endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool ProcessEvent()
        {
            return api.ProcessEvent();
        }
    }
}