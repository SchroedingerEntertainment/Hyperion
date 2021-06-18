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
    public interface ITrayIcon
    {
        /// <summary>
        /// 
        /// </summary>
        Icon Icon
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Tooltip
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool Visible
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        void SetIcon(Icon icon);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tooltip"></param>
        void SetTooltip(string tooltip);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        void SetVisible(bool visible);
    }
}
