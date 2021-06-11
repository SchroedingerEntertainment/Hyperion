// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// The POINT structure defines the x- and y- coordinates of a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        ///  The x-coordinate of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int y;

        /// <summary>
        /// Creates a new Win32 POINT instance
        /// </summary>
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Converts the Win32 POINT type into System.Drawing.Point
        /// </summary>
        public System.Drawing.Point ToPoint()
        {
            return new System.Drawing.Point(x, y);
        }
        /// <summary>
        /// Converts the Win32 POINT type into System.Drawing.PointF
        /// </summary>
        public System.Drawing.PointF ToPointF()
        {
            return new System.Drawing.PointF(x, y);
        }
    }
}
