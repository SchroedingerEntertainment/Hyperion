// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFocusEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        void OnActivated();

        /// <summary>
        /// 
        /// </summary>
        void OnGotFocus();

        /// <summary>
        /// 
        /// </summary>
        void OnLostFocus();

        /// <summary>
        /// 
        /// </summary>
        void OnDeactivate();
    }
}
