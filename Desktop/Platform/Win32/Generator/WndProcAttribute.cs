// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// Marks a method be part of a WndProc proxy
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class WndProcAttribute : Attribute
    {
        protected WindowMessage message;
        /// <summary>
        /// 
        /// </summary>
        public WindowMessage Message
        {
            get { return message; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        public WndProcAttribute(WindowMessage message)
        {
            this.message = message;
        }
    }
}
