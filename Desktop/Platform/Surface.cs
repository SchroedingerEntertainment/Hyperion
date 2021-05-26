// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// A planar area, managed by the operation system
    /// </summary>
    public abstract class Surface
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
        public virtual void OnMove()
        { }

        public virtual void OnResize()
        { }
        #endregion

        #region Appearance
        public virtual void OnIconChanged()
        { }

        public virtual void OnTitleChanged()
        { }

        public virtual void OnBorderChanged()
        { }

        public virtual void OnStateChanged()
        { }
        #endregion

        #region Key Events
        public virtual void OnKeyDown()
        { }

        public virtual void OnKeyPress()
        { }

        public virtual void OnKeyUp()
        { }
        #endregion

        #region Mouse Events
        public virtual void OnMouseEnter()
        { }

        public virtual void OnMouseLeave()
        { }

        public virtual void OnMouseDown()
        { }

        public virtual void OnMouseUp()
        { }

        public virtual void OnMouseMove()
        { }

        public virtual void OnMouseWheel()
        { }
        #endregion

        #region Drag n Drop
        public virtual void OnDragEnter()
        { }

        public virtual void OnDragDrop()
        { }

        public virtual void OnDragLeave()
        { }
        #endregion

        public virtual void OnClosed()
        { }
        #endregion

        /// <summary>
        /// Adds a new Surface instance to the manager
        /// </summary>
        /// <returns>True if not associated to an instance and a new one was
        /// created, false otherwise"/></returns>
        public abstract bool Create();
        
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