using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataPlotter
{
    partial class PlotWindow : UserControl
    {
        #region private variables
        private List<SingleGraph> graphs_;
        int selected_;
        #endregion

        #region public constructors
        public PlotWindow()
        {
            graphs_ = new List<SingleGraph>();
            selected_ = -1;
            InitializeComponent();
        }
        #endregion

        #region public properties
        public int GraphCount
        {
            get { return graphs_.Count; }
        }

        public List<GraphConfig> Configs
        {
            get
            {
                List<GraphConfig> configs = new List<GraphConfig>();
                foreach (SingleGraph gr in graphs_)
                    configs.Add(gr.Config);

                return configs;
            }
        }
        #endregion

        #region public methods
        public void RemoveIfSession(DataSession session)
        {
            List<SingleGraph> torm = new List<SingleGraph>();
            foreach(SingleGraph graph in graphs_)
            {
                if (graph.Session == session)
                    torm.Add(graph);
            }

            foreach (SingleGraph graph in torm)
                DeleteGraph(graph);
        }

        public void AddGraph(DataManager mgr, string xvar)
        {
            if (graphs_.Count == 9)
            {
                MessageBox.Show("Nine is the maximum number of graphs allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SingleGraph graph = new WinChartGraph(graphs_.Count, mgr, xvar);
            graph.Activated += GraphWindowActivated;
            graph.OrderChangeRequest += GraphOrderChangeRequested;
            graph.Parent = this;
            Controls.Add(graph);
            graph.Visible = true;
            graph.Enabled = true;

            graphs_.Add(graph);

            DoLayout();

            selected_ = graphs_.Count - 1;
            HighlightSelected();
        }


        public void AddGraphs(DataManager mgr, List<GraphConfig> configs)
        {
            foreach (GraphConfig c in configs)
            {
                SingleGraph graph = new WinChartGraph(graphs_.Count, mgr, c);
                graph.Activated += GraphWindowActivated;
                graph.OrderChangeRequest += GraphOrderChangeRequested;
                graph.Parent = this;
                Controls.Add(graph);
                graph.Visible = true;
                graph.Enabled = true;
                graphs_.Add(graph);
            }

            DoLayout();
        }

        public void Clear()
        {
            while (graphs_.Count > 0)
                RemoveGraph(graphs_[0]);
        }

        public void DeleteSelected()
        {
            if (selected_ != -1)
            {
                SingleGraph gr = graphs_[selected_];
                RemoveGraph(gr);
                selected_ = -1;
                HighlightSelected();
            }
        }

        public void DeleteGraph(SingleGraph g)
        {
            RemoveGraph(g);
            selected_ = -1;
            HighlightSelected();
        }

        public void UnselectGraph()
        {
            selected_ = -1;
            HighlightSelected();
        }
        #endregion

        #region protected window event methods
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            DoLayout();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

        }
        #endregion

        #region child event handlers
        private void GraphWindowActivated(object sender, EventArgs e)
        {
            SingleGraph gr = sender as SingleGraph;
            if (gr != null)
            {
                selected_ = graphs_.IndexOf(gr);
                HighlightSelected();
            }
        }

        private void GraphOrderChangeRequested(object sender, OrderChangeEventArgs e)
        {
            SingleGraph gr = graphs_[e.Source];
            graphs_.RemoveAt(e.Source);
            graphs_.Insert(e.Destination, gr);
            DoLayout();
        }
        #endregion

        #region private methods

        private void RemoveGraph(SingleGraph gr)
        {
            gr.Activated -= GraphWindowActivated;
            gr.OrderChangeRequest -= GraphOrderChangeRequested;
            graphs_.Remove(gr);
            gr.Cleanup();
            Controls.Remove(gr);
            gr.Parent = null;
            gr.Dispose();
            DoLayout();
        }

        private void HighlightSelected()
        {
            foreach (SingleGraph gr in graphs_)
                gr.BorderStyle = BorderStyle.None;

            if (selected_ >= 0 && selected_ <= graphs_.Count)
                graphs_[selected_].BorderStyle = BorderStyle.FixedSingle;
        }

        private void DoLayout()
        {
            if (graphs_.Count == 1)
            {
                // Single window docked in graph space
                graphs_[0].Dock = DockStyle.Fill;
            }
            else if (graphs_.Count == 2)
            {
                // Two stacked vertically
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width ;
                graphs_[0].Height = Height / 2;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width;
                graphs_[1].Height = Height / 2;
                graphs_[1].Location = new Point(0, Height / 2);
                graphs_[1].Anchor = AnchorStyles.Right| AnchorStyles.Bottom | AnchorStyles.Right;
            }
            else if (graphs_.Count == 3)
            {
                // First two on top side by side, third below
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 2;
                graphs_[0].Height = Height / 2;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 2;
                graphs_[1].Height = Height / 2;
                graphs_[1].Location = new Point(Width / 2, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width;
                graphs_[2].Height = Height / 2;
                graphs_[2].Location = new Point(0, Height / 2);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 4)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 2;
                graphs_[0].Height = Height / 2;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 2;
                graphs_[1].Height = Height / 2;
                graphs_[1].Location = new Point(Width / 2, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 2;
                graphs_[2].Height = Height / 2;
                graphs_[2].Location = new Point(0, Height / 2);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 2;
                graphs_[3].Height = Height / 2;
                graphs_[3].Location = new Point(Width/2, Height / 2);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 5)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 3;
                graphs_[0].Height = Height / 2;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 3;
                graphs_[1].Height = Height / 2;
                graphs_[1].Location = new Point(Width / 3, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 3;
                graphs_[2].Height = Height / 2;
                graphs_[2].Location = new Point(2 * Width / 3, 0);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 2;
                graphs_[3].Height = Height / 2;
                graphs_[3].Location = new Point(0, Height / 2);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[4].Dock = DockStyle.None;
                graphs_[4].Width = Width / 2;
                graphs_[4].Height = Height / 2;
                graphs_[4].Location = new Point(Width / 2, Height / 2);
                graphs_[4].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 6)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 3;
                graphs_[0].Height = Height / 2;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 3;
                graphs_[1].Height = Height / 2;
                graphs_[1].Location = new Point(Width / 3, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 3;
                graphs_[2].Height = Height / 2;
                graphs_[2].Location = new Point(2 * Width / 3, 0);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                
                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 3;
                graphs_[3].Height = Height / 2;
                graphs_[3].Location = new Point(0, Height/2);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[4].Dock = DockStyle.None;
                graphs_[4].Width = Width / 3;
                graphs_[4].Height = Height / 2;
                graphs_[4].Location = new Point(Width / 3, Height /2);
                graphs_[4].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[5].Dock = DockStyle.None;
                graphs_[5].Width = Width / 3;
                graphs_[5].Height = Height / 2;
                graphs_[5].Location = new Point(2 * Width / 3, Height /2);
                graphs_[5].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 7)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 3;
                graphs_[0].Height = Height / 3;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 3;
                graphs_[1].Height = Height / 3;
                graphs_[1].Location = new Point(Width / 3, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 3;
                graphs_[2].Height = Height / 3;
                graphs_[2].Location = new Point(2 * Width / 3, 0);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 3;
                graphs_[3].Height = Height / 3;
                graphs_[3].Location = new Point(0, Height / 3);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[4].Dock = DockStyle.None;
                graphs_[4].Width = Width / 3;
                graphs_[4].Height = Height / 3;
                graphs_[4].Location = new Point(Width / 3, Height / 3);
                graphs_[4].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[5].Dock = DockStyle.None;
                graphs_[5].Width = Width / 3;
                graphs_[5].Height = Height / 3;
                graphs_[5].Location = new Point(2 * Width / 3, Height / 3);
                graphs_[5].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[6].Dock = DockStyle.None;
                graphs_[6].Width = Width ;
                graphs_[6].Height = Height / 3;
                graphs_[6].Location = new Point(0, 2 * Height / 3);
                graphs_[6].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 8)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 3;
                graphs_[0].Height = Height / 3;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 3;
                graphs_[1].Height = Height / 3;
                graphs_[1].Location = new Point(Width / 3, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 3;
                graphs_[2].Height = Height / 3;
                graphs_[2].Location = new Point(2 * Width / 3, 0);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 3;
                graphs_[3].Height = Height / 3;
                graphs_[3].Location = new Point(0, Height / 3);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[4].Dock = DockStyle.None;
                graphs_[4].Width = Width / 3;
                graphs_[4].Height = Height / 3;
                graphs_[4].Location = new Point(Width / 3, Height / 3);
                graphs_[4].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[5].Dock = DockStyle.None;
                graphs_[5].Width = Width / 3;
                graphs_[5].Height = Height / 3;
                graphs_[5].Location = new Point(2 * Width / 3, Height / 3);
                graphs_[5].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[6].Dock = DockStyle.None;
                graphs_[6].Width = Width /2;
                graphs_[6].Height = Height / 3;
                graphs_[6].Location = new Point(0, 2 * Height / 3);
                graphs_[6].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[7].Dock = DockStyle.None;
                graphs_[7].Width = Width / 2;
                graphs_[7].Height = Height / 3;
                graphs_[7].Location = new Point(Width / 2, 2 * Height / 3);
                graphs_[7].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else if (graphs_.Count == 9)
            {
                // Four windows in a two by two array
                graphs_[0].Dock = DockStyle.None;
                graphs_[0].Width = Width / 3;
                graphs_[0].Height = Height / 3;
                graphs_[0].Location = new Point(0, 0);
                graphs_[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[1].Dock = DockStyle.None;
                graphs_[1].Width = Width / 3;
                graphs_[1].Height = Height / 3;
                graphs_[1].Location = new Point(Width / 3, 0);
                graphs_[1].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[2].Dock = DockStyle.None;
                graphs_[2].Width = Width / 3;
                graphs_[2].Height = Height / 3;
                graphs_[2].Location = new Point(2 * Width / 3, 0);
                graphs_[2].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[3].Dock = DockStyle.None;
                graphs_[3].Width = Width / 3;
                graphs_[3].Height = Height / 3;
                graphs_[3].Location = new Point(0, Height / 3);
                graphs_[3].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[4].Dock = DockStyle.None;
                graphs_[4].Width = Width / 3;
                graphs_[4].Height = Height / 3;
                graphs_[4].Location = new Point(Width / 3, Height / 3);
                graphs_[4].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[5].Dock = DockStyle.None;
                graphs_[5].Width = Width / 3;
                graphs_[5].Height = Height / 3;
                graphs_[5].Location = new Point(2 * Width / 3, Height / 3);
                graphs_[5].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[6].Dock = DockStyle.None;
                graphs_[6].Width = Width / 3;
                graphs_[6].Height = Height / 3;
                graphs_[6].Location = new Point(0, 2 * Height / 3);
                graphs_[6].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[7].Dock = DockStyle.None;
                graphs_[7].Width = Width / 3;
                graphs_[7].Height = Height / 3;
                graphs_[7].Location = new Point(Width / 3, 2 * Height / 3);
                graphs_[7].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                graphs_[8].Dock = DockStyle.None;
                graphs_[8].Width = Width / 3;
                graphs_[8].Height = Height / 3;
                graphs_[8].Location = new Point(2 * Width / 3, 2 * Height / 3);
                graphs_[8].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
        }
        #endregion
    }
}
