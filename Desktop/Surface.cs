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
    public abstract class Surface : FinalizerObject, ISurface
    {
        public abstract IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
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

        public abstract bool SizeMove
        {
            get;
        }

        public abstract bool Dirty
        {
            get;
        }

        public virtual SurfaceFlags Flags
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetFlags(value); }
        }

        public virtual TransparencyMask Transparency
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetTransparency(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual WindowState State
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetState(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Title
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { throw new NotImplementedException(); }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { SetTitle(value); }
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

        static Surface()
        {
            #if DEBUG
            try
            {
            #endif
                if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
                {
                    //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                    Compositor.DeclareType<Surface>(typeof(Win32.Surface), typeof(Win32.Renderer));
                }
                else Compositor.DeclareType<Surface>(typeof(X11.Surface), typeof(Renderer));
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
        { }

        /// <summary>
        /// 
        /// </summary>
        public abstract bool Initialize();

        public abstract void SetBounds(int x, int y, int width, int height);

        public abstract void SetFlags(SurfaceFlags mask);

        public abstract void SetTransparency(TransparencyMask transparency);

        public abstract void SetState(WindowState state);

        public abstract void SetTitle(string title);

        public abstract void SetVisible(bool visible);

        public abstract Point PointToClient(Point pt);
        public abstract PointF PointToClient(PointF pt);

        public abstract Point PointToScreen(Point pt);
        public abstract PointF PointToScreen(PointF pt);

        public abstract bool Redraw();

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