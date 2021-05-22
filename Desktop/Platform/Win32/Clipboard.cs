// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Threading;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Clipboard : IClipboard
    {
        struct ClipboardHandle : IDisposable
        {
            public void Dispose()
            {
                CloseClipboard();
            }
        }

        const int MaxRetryCount = 100; //times
        const int MaxClipboardDelay = 1000; //ms

        const int ClipboardDelay = MaxClipboardDelay / MaxRetryCount;

        public IDisposable Open()
        {
            for (int i = MaxRetryCount; i >= 0 && !OpenClipboard(IntPtr.Zero); i--)
            {
                if(i == 0)
                {
                    throw new SynchronizationLockException();
                }
                else Thread.Sleep(ClipboardDelay);
            }
            return new ClipboardHandle();
        }

        public bool Clear()
        {
            using (Open())
            {
                return EmptyClipboard();
            }
        }
    }
}
