// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Threading;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// A planar area, managed by the operation system
    /// </summary>
    public abstract class Surface : FinalizerObject
    {
        protected IntPtr handle;
        /// <summary>
        /// The Surface handle
        /// </summary>
        public IntPtr Handle
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return handle; }
        }
        
        protected atomic_bool dirtyFlag;
        /// <summary>
        /// Gets the state of the dirty flag
        /// </summary>
        public bool Dirty
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return dirtyFlag.UnsafeValue; }
        }

        protected bool sizeMoveFlag;
        /// <summary>
        /// Gets the state of the size-move flag
        /// </summary>
        public bool SizeMove
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return sizeMoveFlag; }
        }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public abstract Color Color { get; set; }

        /// <summary>
        /// Gets or sets if the Surface is enabled
        /// </summary>
        public abstract bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets if the Surface is visible
        /// </summary>
        public abstract bool Visible { get; set; }

        #region Positioning
        /// <summary>
        /// Gets or sets the Surface rect from prosition (x, y) and size (w, h)
        /// </summary>
        public abstract RectangleF Rect { get; set; }

        /// <summary>
        /// Gets the Surface rect from prosition (x, y) and size (w, h) without
        /// titlebar and borders
        /// </summary>
        public abstract RectangleF ClientRect { get; }

        /// <summary>
        /// Gets or sets if the Surface supports user resizing
        /// </summary>
        public abstract bool CanResize { get; set; }

        /// <summary>
        /// Gets or Sets if the Surface should stay always on top
        /// </summary>
        public abstract bool TopMost { get; set; }
        #endregion

        #region Appearance
        /// <summary>
        /// Gets or sets the icon shown in the task- and/or titlebar
        /// </summary>
        public abstract Icon Icon { get; set; }

        /// <summary>
        /// Gets or sets the text shown in the task- and/or titlebar
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// Gets or sets the buttons shown in the titlebar (if any)
        /// </summary>
        public abstract WindowButons Buttons { get; set; }
        
        /// <summary>
        /// Gets or sets if the Surface should has a titlebar
        /// </summary>
        public abstract bool HasTitle { get; set; }
        
        /// <summary>
        /// Gets or sets if the Surface has a frame border
        /// </summary>
        public abstract bool HasBorder { get; set; }
        
        /// <summary>
        /// Gets or sets the management state of this Surface
        /// </summary>
        public abstract WindowState State { get; set; }

        /// <summary>
        /// Gets or sets if the Surface should appear in the taskbar
        /// </summary>
        public abstract bool AppearsInTaskbar { get; set; }
        #endregion

        /// <summary>
        /// Creates a new Surface instance
        /// </summary>
        public Surface()
        { }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static implicit operator bool(Surface surface)
        {
            return (surface.Handle != IntPtr.Zero);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static implicit operator Graphics(Surface surface)
        {
            return Graphics.FromHwnd(surface.handle);
        }

        #region Events
        public virtual void OnColorChanged()
        { }

        public virtual void OnEnabledChanged()
        { }

        public virtual void OnFocusChanged()
        { }

        public virtual void OnVisibleChanged()
        { }

        #region Positioning
        protected virtual void OnMove()
        { }

        protected virtual void OnResize()
        { }
        #endregion

        #region Appearance
        protected virtual void OnIconChanged()
        { }

        protected virtual void OnTitleChanged()
        { }

        protected virtual void OnBorderChanged()
        { }

        protected virtual void OnStateChanged()
        { }
        #endregion

        #region Key Events
        protected virtual void OnKeyDown()
        { }

        protected virtual void OnKeyPress()
        { }

        protected virtual void OnKeyUp()
        { }
        #endregion

        #region Mouse Events
        protected virtual void OnMouseEnter()
        { }

        protected virtual void OnMouseLeave()
        { }

        protected virtual void OnMouseDown()
        { }

        protected virtual void OnMouseUp()
        { }

        protected virtual void OnMouseMove()
        { }

        protected virtual void OnMouseWheel()
        { }
        #endregion

        #region Drag n Drop
        protected virtual void OnDragEnter()
        { }

        protected virtual void OnDragDrop()
        { }

        protected virtual void OnDragLeave()
        { }
        #endregion

        protected virtual void OnFlushBuffer()
        { }

        protected virtual void OnClosed()
        { }
        #endregion

        /// <summary>
        /// Adds a new Surface instance to the manager
        /// </summary>
        /// <returns>True if not associated to an instance and a new one was
        /// created, false otherwise"/></returns>
        public abstract bool Create();

        /// <summary>
        /// Enables the Surface transparency level if possible
        /// </summary>
        /// <param name="mask">The desired transparency level</param>
        /// <returns>The supported transparency level set from the platform</returns>
        public abstract TransparencyMask SetTransparencyMask(TransparencyMask mask);

        /// <summary>
        /// Tries to process the next outstanding message from the message queue
        /// </summary>
        /// <returns>True if all messages have been processed properly, false otherwise</returns>
        public abstract bool ProcessEvent();

        /// <summary>
        /// Begins processing of an outstanding repaint request and clears the dirty flag
        /// </summary>
        /// <returns>True if a request has been processed, false otherwise</returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public bool ProcessRepaint()
        {
            if (dirtyFlag.Exchange(false))
            {
                OnFlushBuffer();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Translates the provided point into Sufrace coordinates
        /// </summary>
        /// <param name="pt">The point to translate</param>
        /// <returns>The translated point</returns>
        public abstract PointF PointToClient(PointF pt);

        /// <summary>
        /// Translates the provided point into Screen coordinates
        /// </summary>
        /// <param name="pt">The point to translate</param>
        /// <returns>The translated point</returns>
        public abstract PointF PointToScreen(PointF pt);

        /// <summary>
        /// Removes this Surface from the manager
        /// </summary>
        public abstract void Close();
    }
}