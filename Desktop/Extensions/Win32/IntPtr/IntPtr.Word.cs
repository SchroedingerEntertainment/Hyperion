// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop.Win32
{
    public static partial class IntPtrExtension
    {
        /// <summary>
        /// Obtains the high order word of this pointer
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static int HiWord(this IntPtr ptr)
        {
            long number = unchecked((long)ptr);
            if ((number & 0x80000000) == 0x80000000)
            {
                return (short)(number >> 16);
            }
            else return (short)(number >> 16) & 0xffff;
        }

        /// <summary>
        /// Obtains the low order word of this pointer
        /// </summary>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static int LoWord(this IntPtr ptr)
        {
            return (short)(unchecked((long)ptr) & 0xffff);
        }
    }
}
