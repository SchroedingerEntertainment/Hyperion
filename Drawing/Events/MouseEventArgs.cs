// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SE.Hyperion.Desktop;

namespace SE.Hyperion.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct MouseEventArgs
    {
        [FieldOffset(0)]
        private readonly UInt64 value;

        [FieldOffset(0)]
        private readonly UInt16 x;
        [FieldOffset(3)]
        private readonly UInt16 y;

        [FieldOffset(7)]
        private readonly MouseButton button;

        /// <summary>
        /// 
        /// </summary>
        public MouseButton Button
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return button; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point Cursor
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return new Point((Int16)x, (Int16)y); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="cursor"></param>
        public MouseEventArgs(MouseButton button, Point cursor)
        {
            this.value = 0;
            this.x = (UInt16)cursor.X;
            this.y = (UInt16)cursor.Y;
            this.button = button;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public int CompareTo(MouseEventArgs other)
        {
            return value.CompareTo(other.value);
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override string ToString()
        {
            return string.Format("Button: {0}, Cursor: {1}", Button, Cursor);
        }
    }
}
