// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class IntPtrExtension
    {
        /// <summary>
        /// Converts this pointer value to a 2D Point
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static System.Drawing.Point ToPoint(this IntPtr ptr)
        {
            return new System.Drawing.Point(ptr.LoWord(), ptr.HiWord());
        }
    }
}
