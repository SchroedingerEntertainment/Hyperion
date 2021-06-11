// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// 
        /// </summary>
        WindowState State
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetTitle();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        void SetTitle(string title);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newState"></param>
        void SetState(WindowState state);
    }
}
