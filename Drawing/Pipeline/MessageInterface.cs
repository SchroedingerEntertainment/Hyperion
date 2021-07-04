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
    /// <summary>
    /// 
    /// </summary>
    public abstract class MessageInterface
    {
        private static readonly MessageInterface instance;

        static MessageInterface()
        {
            #if DEBUG
            try
            {
            #endif
                if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
                {
                    //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                    Compositor.DeclareType<MessageInterface>(typeof(Desktop.Win32.Platform));
                }
                else Compositor.DeclareType<MessageInterface>(typeof(Desktop.X11.Platform));
                instance = Compositor.CreateInstance<MessageInterface>();
            #if DEBUG
            }
            catch (Exception er)
            {
                throw er;
            }
            #endif
        }
        protected MessageInterface()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool GetNext()
        {
            return instance.ProcessEvent();
        }

        protected abstract bool ProcessEvent();

        protected virtual void OnTrayEvent(long id, TrayEvent e, Point cursor)
        {
            switch (e)
            {
                case TrayEvent.Click:
                    {
                        TrayIcon.ClickEvent.Invoke(id, cursor);
                    }
                    break;
                case TrayEvent.DoubleClick:
                    {
                        TrayIcon.DoubleClickEvent.Invoke(id, cursor);
                    }
                    break;
                case TrayEvent.RightClick:
                    {
                        TrayIcon.RightClickEvent.Invoke(id, cursor);
                    }
                    break;
            }
        }
    }
}