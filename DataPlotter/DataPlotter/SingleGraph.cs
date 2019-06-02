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
    abstract partial class SingleGraph : UserControl
    {
        #region public events
        public event EventHandler<EventArgs> Activated;
        public event EventHandler<OrderChangeEventArgs> OrderChangeRequest;

        #endregion

        #region private variables
        private DataManager mgr_;
        private GraphConfig config_;
        private int index_;
        #endregion

        #region protected delegate defintions
        protected delegate void UpdateGraphDelegate();
        protected delegate void ResetGraphDelegate();
        #endregion

        #region public constructor
        public SingleGraph(int index, DataManager mgr, string xvar)
        {
            index_ = index;
            mgr_ = mgr;
            config_ = new GraphConfig(xvar);
            InitializeComponent();
        }

        public SingleGraph(int index, DataManager mgr, GraphConfig c)
        {
            index_ = index;
            mgr_ = mgr;
            config_ = c;
            InitializeComponent();
        }
        #endregion

        #region public methods
        public abstract void Cleanup();
        #endregion

        #region public properties
        public int Index
        {
            get { return index_; }
            set { index_ = value; }
        }

        public DataSession Session
        {
            get { return config_.Session; }
        }
        #endregion

        #region protected properties
        protected DataManager AppDataManager
        {
            get { return mgr_; }
        }

        public GraphConfig Config
        {
            get { return config_; }
        }

        #endregion

        #region protected methods
        protected virtual void OnActivated(EventArgs e)
        {
            Activated?.Invoke(this, e);
        }

        protected virtual void OnOrderChangeRequested(OrderChangeEventArgs e)
        {
            OrderChangeRequest?.Invoke(this, e);
        }
        #endregion
    }
}
