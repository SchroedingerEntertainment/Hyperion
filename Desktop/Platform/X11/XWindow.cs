// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop.X11
{
    public class Window : Surface
    {
        public override Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override RectangleF Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override RectangleF ClientRect => throw new NotImplementedException();

        public override bool CanResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Icon Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override WindowButons Buttons { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool HasTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool HasBorder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override WindowState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool AppearsInTaskbar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool TopMost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool Create()
        {
            throw new NotImplementedException();
        }

        public override PointF PointToClient(PointF pt)
        {
            throw new NotImplementedException();
        }

        public override PointF PointToScreen(PointF pt)
        {
            throw new NotImplementedException();
        }
        
        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override bool ProcessEvent()
        {
            throw new NotImplementedException();
        }

        public override TransparencyMask SetTransparencyMask(TransparencyMask mask)
        {
            throw new NotImplementedException();
        }
    }
}