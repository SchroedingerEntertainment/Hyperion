// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKeyboardEventTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        void OnKeyDown(Key key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        void OnKeyUp(Key key);
    }
}