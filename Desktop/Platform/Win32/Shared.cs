// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop.Win32
{
    internal partial class Shared
    {
        private readonly static Version osVersion;
        /// <summary>
        /// 
        /// </summary>
        public static Version OsVersion
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return osVersion; }
        }

        static Shared()
        {
            OsVersionInfoEx osverinfo = OsVersionInfoEx.Create();
            if (RtlGetVersion(ref osverinfo) == 0)
            {
                osVersion = new Version(osverinfo.dwMajorVersion, osverinfo.dwMinorVersion, osverinfo.dwBuildNumber);
            }
            else osVersion = new Version();
        }
    }
}
