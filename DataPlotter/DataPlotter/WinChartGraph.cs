using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataPlotter
{
    partial class WinChartGraph : SingleGraph
    {
        #region private member variables
        private Chart chart_;
        private DataSession session_;
        private Color defback_;

        #endregion

        #region private constants
        private string EmptyText = "Empty";
        #endregion

        #region public constructors
        public WinChartGraph(int index, DataManager mgr, string xvar) : base(index, mgr, xvar)
        {
            InitializeComponent();
            CreateChart();
            mgr.SessionStarted += DataManagerSessionStarted;
        }

        public WinChartGraph(int index, DataManager mgr, GraphConfig c) : base(index, mgr, c)
        {
            InitializeComponent();
            CreateChart();
            mgr.SessionStarted += DataManagerSessionStarted;

            if (mgr.HasSession(c.SessionName))
            {
                DataSession s = mgr[c.SessionName];
                ResolveSession(s);
            }
        }
        #endregion

        #region public properties

        public bool HasUnresolvedSession
        {
            get
            {
                bool ret = false;

                foreach(GraphVariable v in Config.Variables)
                {
                    if (!v.IsResolved)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }
        }
        #endregion

        #region public methods
        public override void Cleanup()
        {
            AppDataManager.SessionStarted -= DataManagerSessionStarted;
            DisconnectFromSession();
        }
        #endregion

        #region event handlers for child controls
        private void ChartDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void ChartDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void ChartDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
            {
                string varlist = (string)e.Data.GetData(typeof(System.String));
                string[] tokens = varlist.Split(',');

                DataSession session = AppDataManager[tokens[0]];
                if (session_ != null && session != session_)
                {
                    string msg = "Cannot add values from session '" + tokens[0] + "'.";
                    msg += "This graph is already attached to session '" + session_.Name + "'";
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (session_ == null)
                    ConnectToSession(session);

                if ((e.KeyState & 4) == 4)
                    DropXYVariable(tokens);
                else
                    DropYVariable(tokens);


                UpdateGraph();
            }
            else if (e.Data.GetDataPresent(typeof(System.Int32)))
            {
                int index = (int)e.Data.GetData(typeof(System.Int32));
                if (index != Index)
                    OnOrderChangeRequested(new OrderChangeEventArgs(index, Index));
            }
        }

        private void ChartMouseDownHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnActivated(EventArgs.Empty);
                DoDragDrop(Index, DragDropEffects.Move);
            }

            else if (e.Button == MouseButtons.Right)
            {
                if (chart_.Legends.Count == 0)
                    return;

                HitTestResult result = chart_.HitTest(e.X, e.Y);
                if (result.ChartElementType == ChartElementType.LegendItem)
                {
                    LegendCell cell = result.SubObject as LegendCell;
                    if (cell != null)
                    {
                        RemoveElement(cell.Text);
                    }
                }
            }
        }

        private void ChartMouseWheelHandler(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region session event handlers
        private void Session_SessionReset(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                ResetGraphDelegate d = new ResetGraphDelegate(ResetGraph);
                Invoke(d);
            }
            else
            {
                ResetGraph();
            }
        }

        private void Session_DataRowAdded(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                UpdateGraphDelegate d = new UpdateGraphDelegate(UpdateGraph);
                Invoke(d);
            }
            else
            {
                UpdateGraph();
            }
        }

        private void DataManagerSessionStarted(object sender, DataManagerEventArgs e)
        {
            if (HasUnresolvedSession)
            {
                ResolveSession(e.Session);
            }
        }
        #endregion

        #region private methods
        private void DropXYVariable(string[] tokens)
        {
            if (tokens.Length != 3)
                return;

            GraphVariable gvar = new GraphVariable(session_, tokens[1], tokens[2]);
            Config.AddVariable(gvar);
        }

        private void DropYVariable(string[] tokens)
        {
            int index = 1;
            while (index < tokens.Length)
            {
                int col = session_.GetColumnFromName(tokens[index]);
                if (col != -1)
                {
                    GraphVariable gvar = new GraphVariable(session_, Config.XVariable, tokens[index]);
                    Config.AddVariable(gvar);
                }
                index++;
            }
        }

        private void ConnectToSession(DataSession session)
        {
            if (session_ == null)
            {
                session_ = session;
                session_.DataRowAdded += Session_DataRowAdded;
                session_.SessionReset += Session_SessionReset;
            }
        }

        private void DisconnectFromSession()
        {
            if (session_ != null)
            {
                session_.DataRowAdded -= Session_DataRowAdded;
                session_.SessionReset -= Session_SessionReset;
                session_ = null;
            }
        }

        private void CreateChart()
        {
            chart_ = new Chart();
            defback_ = chart_.BackColor;
            chart_.BackColor = Color.LightGray;
            chart_.MouseDown += ChartMouseDownHandler;
            chart_.MouseWheel += ChartMouseWheelHandler;
            chart_.Series.Clear();
            chart_.Dock = DockStyle.Fill;
            chart_.Visible = true;
            chart_.Enabled = true;
            chart_.Parent = this;
            chart_.Titles.Add(EmptyText);
            chart_.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 22.0f);
            Controls.Add(chart_);

            chart_.AllowDrop = true;
            chart_.DragDrop += ChartDragDrop;
            chart_.DragEnter += ChartDragEnter;
            chart_.DragOver += ChartDragOver;
        }


        private void ResolveSession(DataSession s)
        {
            if (Config.ResolveVariables(s) && session_ == null)
                ConnectToSession(s);

            if (InvokeRequired)
            {
                UpdateGraphDelegate d = new UpdateGraphDelegate(UpdateGraph);
                Invoke(d);
            }
            else
            {
                UpdateGraph();
            }
        }

        private void RemoveElement(string name)
        {
            GraphVariable theone = null;

            foreach (GraphVariable gvar in Config.Variables)
            {
                if (gvar.YVariable == name)
                    theone = gvar;
            }

            if (theone != null)
            {
                Config.Variables.Remove(theone);
                chart_.Series.Clear();
                UpdateGraph();
            }
        }

        private void ResetGraph()
        {
            foreach (GraphVariable gvar in Config.Variables)
                gvar.Tag = null;

            chart_.Series.Clear();
        }

        private void UpdateGraph()
        {
            if (chart_.Series.Count == 0)
            {
                chart_.BackColor = defback_;
                chart_.ChartAreas.Clear();
                chart_.ChartAreas.Add(Config.Title);

                ChartArea a = chart_.ChartAreas[0];
                a.BackColor = Color.Gray;
                a.AxisX.Title = Config.XVariable;
                a.AxisX.ScaleView.Zoomable = true;
                a.AxisY.ScaleView.Zoomable = true;
                a.CursorX.AutoScroll = true;
                a.CursorY.AutoScroll = true;
                a.CursorX.IsUserEnabled = true;
                a.CursorY.IsUserEnabled = true;
                a.CursorX.IsUserSelectionEnabled = true;
                a.CursorX.IsUserSelectionEnabled = true;
                a.CursorX.IntervalType = DateTimeIntervalType.Number;
                a.CursorX.Interval = 0;
                a.CursorX.IntervalOffsetType = DateTimeIntervalType.Number;
                a.CursorX.IntervalOffset = 0;

                a.CursorY.IntervalType = DateTimeIntervalType.Auto;
                a.CursorY.Interval = 0;
                a.CursorY.IntervalOffsetType = DateTimeIntervalType.Auto;
                a.CursorY.IntervalOffset = 0;

                if (session_ != null && chart_.Titles.Count == 1 && chart_.Titles[0].Text == EmptyText)
                {
                    chart_.Titles.Clear();
                    chart_.Titles.Add(session_.Name);
                    chart_.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 22.0f);
                }

                chart_.Legends.Clear();
                chart_.Legends.Add("Values");
                chart_.Legends[0].Font = new Font(FontFamily.GenericSansSerif, 18.0f);
            }

            foreach (GraphVariable gvar in Config.Variables)
            {
                if (gvar.IsResolved)
                {
                    //
                    // We only add variables when they are bound to a live session
                    //
                    Series ser;

                    if (gvar.Tag == null)
                    {
                        ser = new Series(gvar.YVariable);
                        ser.ChartType = SeriesChartType.Point;
                        gvar.Tag = ser;
                    }
                    else
                    {
                        ser = gvar.Tag as Series;
                    }

                    if (gvar.XColumn == -1 || gvar.YColumn == -1)
                        gvar.Session = session_;

                    if (gvar.XColumn != -1 && gvar.YColumn != -1)
                    {
                        int i = ser.Points.Count;
                        while (i < gvar.Session.RowCount)
                        {
                            if (gvar.Session.IsPresent(i, gvar.XColumn) && gvar.Session.IsPresent(i, gvar.YColumn))
                            {
                                double xval = gvar.Session.GetValue(i, gvar.XColumn);
                                double yval = gvar.Session.GetValue(i, gvar.YColumn);
                                ser.Points.AddXY(xval, yval);
                                ser.Points[ser.Points.Count - 1].ToolTip = gvar.XVariable + " #VALX\n" + gvar.YVariable + " #VALY";
                            }
                            i++;
                        }

                        if (!chart_.Series.Contains(ser))
                            chart_.Series.Add(ser);
                    }
                }
            }
        }
        #endregion
    }
}
