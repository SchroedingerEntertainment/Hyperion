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
    public interface IRenderer : INative
    {
        /// <summary>
        /// 
        /// </summary>
        Rectangle ClientRect
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        RenderBuffer Buffer
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool Dirty
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool SizeMove
        {
            get;
        }
    }
}
