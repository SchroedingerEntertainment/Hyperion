// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop
{
    public static partial class RectangleExtension
    {
        /// <summary>
        /// Determines if a provided point is located inside this Rectangle
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool Contains(this Rectangle rect, Point pt)
        {
            return (rect.Left >= pt.X && rect.Right <= pt.X && rect.Top >= pt.Y && rect.Bottom <= pt.Y);
        }
        /// <summary>
        /// Determines if a provided point is located inside this Rectangle
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool Contains(this Rectangle rect, PointF pt)
        {
            return (rect.Left >= pt.X && rect.Right <= pt.X && rect.Top >= pt.Y && rect.Bottom <= pt.Y);
        }
        /// <summary>
        /// Determines if a provided point is located inside this Rectangle
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool Contains(this RectangleF rect, Point pt)
        {
            return (rect.Left >= pt.X && rect.Right <= pt.X && rect.Top >= pt.Y && rect.Bottom <= pt.Y);
        }
        /// <summary>
        /// Determines if a provided point is located inside this Rectangle
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool Contains(this RectangleF rect, PointF pt)
        {
            return (rect.Left >= pt.X && rect.Right <= pt.X && rect.Top >= pt.Y && rect.Bottom <= pt.Y);
        }
    }
}
