// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    internal partial class Platform
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

        private readonly static Connector defaultConnector;
        /// <summary>
        /// 
        /// </summary>
        public static IntPtr DefaultConnector
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return defaultConnector.handle; }
        }

        static Platform()
        {
            OsVersionInfoEx osverinfo = OsVersionInfoEx.Create();
            if (RtlGetVersion(ref osverinfo) == 0)
            {
                osVersion = new Version(osverinfo.dwMajorVersion, osverinfo.dwMinorVersion, osverinfo.dwBuildNumber);
            }
            else osVersion = new Version();
            defaultConnector = new Connector(null);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static Exception GetLastWin32Error()
        {
            int errorCode = Marshal.GetLastWin32Error();
            return new ExternalException(System.Runtime.Platform.GetWin32ErrorMessage(errorCode), errorCode);
        }
    }
}
