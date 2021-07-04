// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TaskbarMessageAttribute : WndProcAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public TaskbarMessageAttribute()
            : base(default(WindowMessage))
        {
            this.message = (WindowMessage)Platform.WM_TBRESTART;
        }
    }
}