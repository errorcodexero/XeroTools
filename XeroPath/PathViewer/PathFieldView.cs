﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using XeroMath;

namespace PathViewer
{
    public partial class PathFieldView : BasicFieldView
    {
        #region private constants
        private const double TrianglePercent = 0.03;
        #endregion

        #region private variables
        private double m_center_hit_dist = 8.0;
        private WayPoint m_selected;
        private WayPoint m_rotating;
        private bool m_dragging;
        private float m_radius = 8.0f;
        private PathFile m_file;
        private bool m_last_msg_selected;
        private Nullable<PointF> m_highlight;
        private float m_tw;
        private string m_units;
        #endregion

        #region private enums
        private enum WaypointRegion
        {
            Center,
            RotateFeature,
            None
        };
        #endregion

        #region public events

        /// <summary>
        /// This event is fired when a waypoint is selected with the mouse
        /// </summary>
        public event EventHandler<WaypointEventArgs> WaypointSelected;

        /// <summary>
        /// This event is fired when the X, Y, Heading, or Velocity of a waypoint changes
        /// </summary>
        public event EventHandler<WaypointEventArgs> WaypointChanged;

        #endregion

        #region public constructors
        public PathFieldView()
        {
            m_selected = null;
            m_rotating = null;
            m_dragging = false;
            m_last_msg_selected = false;
            DoubleBuffered = true;

            m_tw = 0;
            m_units = "inches";

            InitializeComponent();
        }
        #endregion

        #region public properties

        public string Units
        {
            get { return m_units; }
            set
            {
                m_tw = (float)UnitConverter.Convert(m_tw, m_units, value);
                m_units = value;
            }
        }

        public Nullable<PointF> HighlightPoint
        {
            get { return m_highlight; }
            set { m_highlight = value; Invalidate(); }
        }

        public WayPoint SelectedWaypoint
        {
            get { return m_selected; }
            set { m_selected = value; Invalidate(); }
        }

        public PathFile File
        {
            get { return m_file; }
            set
            {
                PathFile pf = m_file;

                m_selected = null;
                m_file = value;
                DisplayedPath = null;
            }
        }

        #endregion

        #region public methods
        #endregion

        #region overriden usercontrol methods

        protected override void OnGameChanged(EventArgs args)
        {
            base.OnGameChanged(args);

            if (FieldGame == null)
                m_tw = 10.0f;
            else
                m_tw = (float)(Math.Min(FieldGame.FieldSize[0], FieldGame.FieldSize[1]) * TrianglePercent);
        }

        protected void OnWayPointSelected(WaypointEventArgs args)
        {
            EventHandler<WaypointEventArgs> handler = WaypointSelected;
            handler?.Invoke(this, args);
        }

        protected void OnWayPointChanged(WaypointEventArgs args)
        {
            EventHandler<WaypointEventArgs> handler = WaypointChanged;
            handler?.Invoke(this, args);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            PointF pt = WindowToWorld(new PointF(e.X, e.Y));

            PathGroup gr;
            RobotPath path;

            m_file.FindPathByWaypoint(m_selected, out gr, out path);

            if (m_dragging && m_selected != null)
            {
                if (m_last_msg_selected)
                {
                    if (m_selected != null)
                        OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.StartChange, gr, path, m_selected));

                    m_last_msg_selected = false;
                }

                InvalidateWaypoint(m_selected);
                m_selected.X = pt.X;
                m_selected.Y = pt.Y;
                InvalidateWaypoint(m_selected);

                OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.Changing, gr, path, m_selected));
            }
            else if (m_rotating != null)
            {
                if (m_last_msg_selected)
                {
                    if (m_selected != null)
                        OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.StartChange, gr, path, m_selected));

                    m_last_msg_selected = false;
                }

                m_rotating.Heading = FindAngle(new PointF((float)m_rotating.X, (float)m_rotating.Y), pt);
                InvalidateWaypoint(m_rotating);
                OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.Changing, gr, path, m_rotating));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            PathGroup gr = null;
            RobotPath path = null;

            base.OnMouseUp(e);

            Capture = false;
            if (m_dragging)
            {
                m_file.FindPathByWaypoint(m_selected, out gr, out path);
                m_dragging = false;
                if (!m_last_msg_selected)
                {
                    m_file.FindPathByWaypoint(m_selected, out gr, out path);
                    OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.EndChange, gr, path, m_selected));
                }
                Invalidate();
            }
            else if (m_rotating != null)
            {
                m_selected = m_rotating;
                m_rotating = null;

                if (!m_last_msg_selected)
                {
                    m_file.FindPathByWaypoint(m_selected, out gr, out path);
                    OnWayPointChanged(new WaypointEventArgs(WaypointEventArgs.ReasonType.EndChange, gr, path, m_selected));
                }
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            m_highlight = null;

            WayPoint pt;
            WaypointRegion region;

            if (HitTestWaypoint(e.X, e.Y, out pt, out region))
            {
                PathGroup group;
                RobotPath path;

                Capture = true;

                m_file.FindPathByWaypoint(pt, out group, out path);
                if (region == WaypointRegion.Center)
                {
                    InvalidateWaypoint(m_selected);
                    InvalidateWaypoint(pt);
                    m_selected = pt;
                    m_dragging = true;
                    
                    OnWayPointSelected(new WaypointEventArgs(WaypointEventArgs.ReasonType.Selected, group, path, pt));
                    m_last_msg_selected = true;
                }
                else if (region == WaypointRegion.RotateFeature)
                {
                    m_rotating = pt;
                }
            }
            else
            {
                OnWayPointSelected(new WaypointEventArgs(WaypointEventArgs.ReasonType.Unselected));
                InvalidateWaypoint(m_selected);
                m_selected = null;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DisplayedPath != null)
                DrawPath(e.Graphics, DisplayedPath);

            if (m_highlight != null)
            {
                float radius = 4.0f;
                PointF wpf = WorldToWindow(m_highlight.Value);
                RectangleF r = new RectangleF(wpf, new SizeF(0.0f, 0.0f));
                r.Offset(-radius / 2.0f, -radius / 2.0f);
                r.Inflate(radius, radius);

                using (Brush br = new SolidBrush(Color.AntiqueWhite))
                {
                    e.Graphics.FillEllipse(br, r);
                }
            }
        }
        #endregion

        #region private methods

        private double FindAngle(PointF first, PointF second)
        {
            double angle = Math.Atan2(second.Y - first.Y, second.X - first.X);
            return XeroUtils.BoundDegrees(XeroUtils.RadiansToDegrees(angle) - 90.0);
        }

        private bool HitTestWaypoint(int x, int y, out WayPoint hitpt, out WaypointRegion part)
        {
            bool found = false;
            hitpt = null;
            part = WaypointRegion.None;

            if (DisplayedPath == null)
                return false;

            //
            // See if there is a point near the point
            //
            foreach (WayPoint pt in DisplayedPath.Points)
            {
                PointF rpt = new PointF((float)pt.X, (float)pt.Y);
                PointF p = WorldToWindow(rpt);

                //
                // Test the center of the waypoint
                //
                double dist = Math.Sqrt((p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y));
                if (dist < m_center_hit_dist)
                {
                    hitpt = pt;
                    part = WaypointRegion.Center;
                    found = true;
                    break;
                }

                if (pt != m_selected)
                    continue;

                //
                // Test the rotation circle
                //
                PointF[] rot = new PointF[] { new PointF(0f, 3.0f * (float)m_file.Robot.Width / 4.0f) };

                Matrix mm;
                mm = new Matrix();
                mm.Translate((float)pt.X, (float)pt.Y);
                mm.Rotate((float)pt.Heading);
                mm.TransformPoints(rot);

                RectangleF rf = new RectangleF(rot[0], new SizeF(0, 0));
                rf.Inflate(5.0f, 5.0f);
                PointF mpt = WindowToWorld(new PointF(x, y));

                Debug.WriteLine("HIT " + rf.ToString() + " " + mpt.ToString());

                if (rf.Contains(mpt))
                {
                    hitpt = pt;
                    part = WaypointRegion.RotateFeature;
                    found = true;
                }
            }

            return found;
        }

        private void InvalidateWaypoint(WayPoint pt)
        {
            Invalidate();
        }

        private RectangleF WaypointBounds(WayPoint pt)
        {
            PointF p = WorldToWindow(new PointF((float)pt.X, (float)pt.Y));
            RectangleF r = new RectangleF(p.X, p.Y, 0.0f, 0.0f);
            r.Inflate(m_radius, m_radius);

            return r;
        }

        private void DrawPath(Graphics g, RobotPath path)
        {
            if (path.HasSplines)
                DrawSplines(g, path);

            DrawPoints(g, path.Points);
        }

        private void DrawSplines(Graphics g, RobotPath path)
        {
            for (int i = 0; i < path.Splines.Length; i++)
                DrawSpline(g, path, i);
        }

        private void DrawSpline(Graphics g, RobotPath path, int index)
        {
            float diam = 2.0f;

            using (Brush b = new SolidBrush(Color.LightCoral))
            {
                double step = 1.0 / 1000.0;
                for (double t = 0.0; t < 1.0; t += step)
                {
                    double x, y, heading;
                    RectangleF rf;
                    double px, py;
                    PointF pt, w;

                    path.Evaluate(index, t, out x, out y, out heading);
                    heading = XeroUtils.DegreesToRadians(heading);
                    double ca = Math.Cos(heading);
                    double sa = Math.Sin(heading);

                    px = x - m_file.Robot.Width * sa / 2.0;
                    py = y + m_file.Robot.Width * ca / 2.0;

                    pt = new PointF((float)px, (float)py);
                    w = WorldToWindow(pt);

                    rf = new RectangleF(w, new SizeF(0.0f, 0.0f));
                    rf.Offset(diam / 2.0f, diam / 2.0f);
                    rf.Inflate(diam, diam);
                    g.FillEllipse(b, rf);

                    px = x + m_file.Robot.Width * sa / 2.0;
                    py = y - m_file.Robot.Width * ca / 2.0;

                    pt = new PointF((float)px, (float)py);
                    w = WorldToWindow(pt);

                    rf = new RectangleF(w, new SizeF(0.0f, 0.0f));
                    rf.Offset(diam / 2.0f, diam / 2.0f);
                    rf.Inflate(diam, diam);
                    g.FillEllipse(b, rf);
                }
            }
        }

        private void DrawOneWaypoint(Graphics g, WayPoint pt, Pen selpen, Pen normpen, Brush selbrush, Brush normbrush)
        {
            PointF[] triangle = new PointF[] { new PointF(m_tw, 0.0f), new PointF(-m_tw / 2.0f, m_tw / 2.0f), new PointF(-m_tw / 2.0f, -m_tw / 2.0f) };

            Matrix mm;
            mm = new Matrix();
            mm.Translate((float)pt.X, (float)pt.Y);
            mm.Rotate((float)pt.Heading);
            mm.TransformPoints(triangle);

            float angle = (float)XeroUtils.DegreesToRadians(pt.Heading);

            //
            // Draw the point
            //
            PointF p = WorldToWindow(new PointF((float)pt.X, (float)pt.Y));

            WorldToWindowMatrix.TransformPoints(triangle);

            g.FillPolygon(normbrush, triangle);
            if (m_selected == pt)
                g.DrawPolygon(selpen, triangle);
            else
                g.DrawPolygon(normpen, triangle);

            //
            // Draw the selection box
            //
            if (m_selected == pt)
            {
                PointF[] selbox = new PointF[]
                {
                    new PointF((float)m_file.Robot.Length / 2.0f, (float)m_file.Robot.Width / 2.0f),
                    new PointF(-(float)m_file.Robot.Length / 2.0f, (float)m_file.Robot.Width / 2.0f),
                    new PointF(-(float)m_file.Robot.Length / 2.0f, -(float)m_file.Robot.Width / 2.0f),
                    new PointF((float)m_file.Robot.Length / 2.0f, -(float)m_file.Robot.Width / 2.0f),
                };

                PointF[] rotateline = new PointF[]
                {
                    new PointF(0f, (float)m_file.Robot.Width / 2.0f),
                    new PointF(0f, 3.0f * (float)m_file.Robot.Width / 4.0f),
                };

                PointF[] circle = new PointF[]
                {
                    new PointF(0f, 3.0f * (float)m_file.Robot.Width / 4.0f),
                    new PointF(0f, (float)m_file.Robot.Width / 4.0f),
                };

                mm.TransformPoints(selbox);
                WorldToWindowMatrix.TransformPoints(selbox);
                g.DrawPolygon(selpen, selbox);

                mm.TransformPoints(rotateline);
                WorldToWindowMatrix.TransformPoints(rotateline);
                g.DrawLine(selpen, rotateline[0], rotateline[1]);

                mm.TransformPoints(circle);
                WorldToWindowMatrix.TransformPoints(circle);

                RectangleF rectf = new RectangleF(circle[0], new SizeF(0, 0));
                rectf.Inflate(5.0f, 5.0f);
                g.FillEllipse(selbrush, rectf);
            }
        }

        private void DrawPoints(Graphics g, WayPoint[] pts)
        {
            using (Pen selpen = new Pen(Color.Yellow, 2.0f))
            {
                using (Brush normbrush = new SolidBrush(Color.Orange))
                {
                    using (Brush selbrush = new SolidBrush(Color.Yellow))
                    {
                        using (Pen normpen = new Pen(Color.Black, 3.0f))
                        {
                            foreach (WayPoint pt in pts)
                                DrawOneWaypoint(g, pt, selpen, normpen, selbrush, normbrush);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
