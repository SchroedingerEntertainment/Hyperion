// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop.Win32
{
    internal partial class Platform
    {
        const string NtDll = "ntdll";

        [DllImport(NtDll, SetLastError = true)]
        public static extern int RtlGetVersion(ref OsVersionInfoEx lpVersionInformation);
    }
}
