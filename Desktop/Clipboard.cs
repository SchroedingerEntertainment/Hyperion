// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop
{
    public class Clipboard
    {
        private static IClipboard instance;

        /*static Clipboard()
        {
            if ((Application.Platform & PlatformName.Windows) == PlatformName.Windows)
            {
                //TODO - Chekc if X11 can be used on Windows (using VcXsrv) as well and provide an option to do so
                instance = new Win32.Clipboard();
            }
            else instance = new X11.Clipboard();
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool Clear()
        {
            return instance.Clear();
        }
    }
}