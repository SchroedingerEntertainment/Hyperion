// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppearanceEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        void OnIconChanged(Icon icon);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        void OnTitleChanged(string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void OnStateChanged(WindowState state);
    }
}
