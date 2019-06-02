using System;
using System.Drawing;

namespace PathViewer
{
    public class FieldMouseMovedArgs : EventArgs
    {
        public readonly PointF WorldPoint;
        public readonly PointF WindowPoint;
        public readonly PointF BackPoint;

        public FieldMouseMovedArgs(PointF world, PointF window, PointF back)
        {
            WorldPoint = world;
            WindowPoint = window;
            BackPoint = back;
        }
    }
}
