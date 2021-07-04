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
    public abstract partial class Surface : FinalizerObject, IRenderer
    {
        public abstract IntPtr Handle
        {
            get;
        }

        public virtual Rectangle Bounds
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetBounds(value.X, value.Y, value.Width, value.Height); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Point Location
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return Bounds.Location; }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set 
            {
                Size size = Bounds.Size;
                SetBounds(value.X, value.Y, size.Width, size.Height); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Size Size
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return Bounds.Size; }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set 
            {
                Point location = Bounds.Location;
                SetBounds(location.X, location.Y, value.Width, value.Height);
            }
        }

        public abstract Rectangle ClientRect
        {
            get;
        }
        
        public abstract RenderBuffer Buffer
        {
            get;
        }
        public abstract bool SizeMove
        {
            get;
        }
        public abstract bool Dirty
        {
            get;
        }

        public abstract Icon Icon
        {
            get;
            set;
        }
        public abstract Transparency Transparency
        {
            get;
            set;
        }
        public abstract WindowState State
        {
            get;
            set;
        }
        public abstract string Title
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
                    Compositor.DeclareType<Surface>
                    (
                        typeof(Desktop.Win32.Window), 
                        typeof(Desktop.Win32.KeyboardComponent), 
                        typeof(Desktop.Win32.MouseComponent),
                        typeof(Desktop.Win32.FocusComponent),
                        typeof(Desktop.Win32.RendererComponent),
                        typeof(SurfaceListener)
                    );
                }
                else Compositor.DeclareType<Surface>(typeof(Desktop.X11.Window), typeof(Renderer));
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
        public Surface()
        {
            //SetAppearance(Appearance.Icon | Appearance.Minimize | Appearance.Maximize | Appearance.Taskbar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected abstract void SetAppearance(Appearance value);

        /// <summary>
        /// 
        /// </summary>
        protected internal abstract bool Initialize();
        
        /// <summary>
        /// 
        /// </summary>
        protected internal abstract bool Redraw();

        public abstract void SetActive();

        public abstract void SetBounds(int x, int y, int width, int height);

        public abstract void SetFocus();

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public void BringToFront()
        {
            SetOrder(true);
        }
        protected abstract void SetOrder(bool top);

        public abstract Point PointToClient(Point pt);
        public abstract PointF PointToClient(PointF pt);

        public abstract Point PointToScreen(Point pt);
        public abstract PointF PointToScreen(PointF pt);

        public abstract void Invalidate();

        public abstract void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static Surface Create()
        {
            return Compositor.CreateInstance<Surface>();
        }
    }
}