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
    public interface ISurface
    {
        /// <summary>
        /// 
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// 
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// 
        /// </summary>
        SurfaceFlags Flags { get; }

        /// <summary>
        /// 
        /// </summary>
        TransparencyMask Transparency { get; }

        /// <summary>
        /// 
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetBounds(int x, int y, int width, int height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        void SetFlags(SurfaceFlags flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transparency"></param>
        void SetTransparency(TransparencyMask transparency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        void SetVisible(bool visible);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Point PointToClient(Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        PointF PointToClient(PointF pt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Point PointToScreen(Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        PointF PointToScreen(PointF pt);

        /// <summary>
        /// 
        /// </summary>
        void Close();
    }
}
