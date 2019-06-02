using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Threading;

namespace PathViewer
{
    public partial class RobotPlotViewer : UserControl
    {

        #region private members
        /// <summary>
        /// The path we are plotting
        /// </summary>
        private RobotPath m_path;

        /// <summary>
        /// The current time for the path
        /// </summary>
        private double m_time;

        /// <summary>
        /// The line annotation visually indicating the current time
        /// </summary>
        private LineAnnotation m_line;

        /// <summary>
        /// The rectangular annotation visuallying giving the current time value
        /// </summary>
        private RectangleAnnotation m_rect;

        /// <summary>
        /// The units being used
        /// </summary>
        private string m_units;
        #endregion

        #region public events
        /// <summary>
        /// Called when the user moves the time cursor
        /// </summary>
        public event EventHandler<TimeCursorMovedArgs> TimeCursorMoved;
        #endregion

        #region private delegates
        /// <summary>
        /// Called to regenerate the series in the graph
        /// </summary>
        private delegate void UpdateGraphSeriesDelegate();

        /// <summary>
        /// Called to set the current time for the plot window
        /// </summary>
        /// <param name="time">the current time for the plot window</param>
        private delegate void SetTimeDelegate(double time);

        /// <summary>
        /// Called to set the path to be plotted
        /// </summary>
        /// <param name="path">the new path for the plot window</param>
        private delegate void SetPathDelegate(RobotPath path);
        #endregion

        #region public constructor
        public RobotPlotViewer()
        {
            m_time = 0.0;

            InitializeComponent();
            DoubleBuffered = true;
            CreateAnnotations();
        }
        #endregion

        #region public properities
        /// <summary>
        /// The units used by the user.
        /// </summary>
        public string Units
        {
            get { return m_units; }
            set { m_units = value; Invalidate(); }
        }

        /// <summary>
        /// The current time.  This is synchronized to the GUI thread so it can be set
        /// from any thread.
        /// </summary>
        public double Time
        {
            get { return m_time; }
            set
            {
                if (InvokeRequired)
                    Invoke(new SetTimeDelegate(SetTime), new object[] { value });
                else
                    SetTime(value);
            }
        }

        /// <summary>
        /// The path to be displayed in the plot window.  This property is not syn
        /// </summary>
        public RobotPath Path
        {
            get { return m_path; }
            set
            {
                if (InvokeRequired)
                    Invoke(new SetPathDelegate(SetPath), new object[] { value });
                else
                    SetPath(value);
            }
        }
        #endregion

        #region private methods
        private void SetPath(RobotPath path)
        {
            if (m_path != null)
                m_path.SegmentsUpdated -= PathSegmentsUpdated;

            m_path = path;
            if (m_path != null)
            {
                m_path.SegmentsUpdated += PathSegmentsUpdated;
                PathSegmentsUpdated(null, null);
                Time = 0.0;
            }
            else
            {
                m_chart.Series.Clear();
            }
        }

        private void CreateAnnotations()
        {
            m_chart.Annotations.Clear();

            if (Path == null || !Path.HasSegments)
            {
                TextAnnotation txt = new TextAnnotation();
                if (Path == null)
                    txt.Text = "No path selected";
                else
                    txt.Text = "Generating detailed path data ...";
                txt.AnchorX = 50;
                txt.AnchorY = 50;
                txt.Font = new Font(FontFamily.GenericSansSerif, 24.0f);
                m_chart.Annotations.Add(txt);
            }
            else
            {
                m_line = new VerticalLineAnnotation();
                m_line.LineColor = Color.Black;
                m_line.LineDashStyle = ChartDashStyle.Dash;
                m_line.AxisX = m_chart.ChartAreas[0].AxisX;
                m_line.IsInfinitive = true;
                m_line.ClipToChartArea = m_chart.ChartAreas[0].Name;
                m_line.LineWidth = 3;
                m_line.AnchorX = m_time;
                m_line.AllowMoving = true;
                m_chart.Annotations.Add(m_line);
                m_chart.AnnotationPositionChanging += TimeBarChangedByUser;

                m_rect = new RectangleAnnotation();
                m_rect.ForeColor = Color.White;
                m_rect.BackColor = Color.Blue;
                m_rect.Width = 10;
                m_rect.Height = 4;
                m_rect.Width = 2.5;
                m_rect.AnchorY = 0.0;
                m_rect.AnchorX = m_time;
                m_rect.AxisY = m_chart.ChartAreas[0].AxisY;
                m_rect.AxisX = m_chart.ChartAreas[0].AxisX;
                m_rect.ClipToChartArea = null;
                m_chart.Annotations.Add(m_rect);
            }
        }

        private void SetTime(double time)
        {
            //
            // I should not have to recreate the annotations every time, but something is not working 
            // right in either how the MS Chart control works, or my understanding of it.  Once a user
            // moves the annotation with a mouse, I can no longer move it programatically.
            //
            CreateAnnotations();

            if (m_rect != null && m_line != null)
            {
                m_time = time;
                m_rect.Text = m_time.ToString("F2");
                m_rect.AnchorX = m_time;
                m_line.AnchorX = m_time;
            }
        }

        private void TimeBarChangedByUser(object sender, AnnotationPositionChangingEventArgs e)
        {
            Debug.Assert(m_line == e.Annotation);
            m_rect.AnchorX = e.NewLocationX;
            EventHandler<TimeCursorMovedArgs> handler = TimeCursorMoved;
            handler?.Invoke(this, new TimeCursorMovedArgs(e.NewLocationX));
        }        

        private void PathSegmentsUpdated(object sender, EventArgs e)
        {
            if (!Created)
                return;

            if (InvokeRequired)
                Invoke(new UpdateGraphSeriesDelegate(UpdateGraphSeries));
            else
                UpdateGraphSeries();
        }

        private void UpdateGraphSeries()
        {
            m_chart.Series.Clear();
            if (m_path != null)
            {
                PathSegment[] segs = m_path.Segments;
                if (segs != null)
                {
                    GenerateDataSeries(segs);
                    Invalidate();
                }
            }
        }

        private void CreateSeries(string name, AxisType a, PathSegment[] segs)
        {
            Series s = new Series(name);
            s.YAxisType = a ;
            s.ChartType = SeriesChartType.Line;
            for(int i = 0; i < segs.Length; i++)
            {
                double t = segs[i].GetValue("time");
                double v = segs[i].GetValue(name);
                s.Points.AddXY(t, v);
                s.BorderWidth = 5;
            }
            s.ToolTip = "#VALX, #VALY";
            m_chart.Series.Add(s);
        }

        private void GenerateDataSeries(PathSegment[] segs)
        {
            ChartArea area = m_chart.ChartAreas[0];

            m_chart.Series.Clear();
            CreateSeries("position", AxisType.Primary, segs);
            CreateSeries("velocity", AxisType.Secondary, segs);
            CreateSeries("acceleration", AxisType.Secondary, segs);

            area.AxisX.Title = "time (sec)";
            area.AxisY.Title = "distance (" + m_units + ")";
            area.AxisY2.Title = "velocity/acceleration";

            m_chart.ChartAreas[0].RecalculateAxesScale();

            CreateAnnotations();
        }
        #endregion
    }
}
