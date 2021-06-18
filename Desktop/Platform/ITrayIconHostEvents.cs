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
    public interface ITrayIconHostEvents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        bool OnTrayEvent(TrayEvent e, Point cursor);
    }
}
