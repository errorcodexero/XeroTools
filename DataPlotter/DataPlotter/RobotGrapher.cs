using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

namespace DataPlotter
{
    partial class RobotGrapher : Form
    {
        #region private contants
        private const string ActiveString = " [Running]";
        #endregion

        #region private member variables
        private DataManager datamgr_;
        private ContextMenu sessionmenu_;
        private string activesession_;
        #endregion

        #region private delegate defintions
        private delegate void DataSessionDelegate(DataSession session);
        #endregion

        #region public constructor
        public RobotGrapher()
        {
            datamgr_ = new DataManager();
            datamgr_.Start();

            InitializeComponent();

            MenuItem[] items = new MenuItem[3];
            items[0] = new MenuItem("Save...");
            items[0].Click += SessionSaveMenuHandler;
            items[1] = new MenuItem("View...");
            items[1].Click += SessionViewMenuHandler;
            items[2] = new MenuItem("Delete");
            items[2].Click += SessionDeleteMenuHandler;
            sessionmenu_ = new ContextMenu(items);
            sessionmenu_.Collapse += SessionMenuClosed;

            datamgr_.SessionStarted += NewSessionStarted;
            datamgr_.SessionFinished += SessionFinished;
            sessions_.ItemSelectionChanged += SelectedSessionChanged;
            sessions_.MouseDown += SessionContextMenuRequest;
            variables_.ItemDrag += DragVariableItem;

            this.KeyDown += RobotGrapherKeyDown;

            sessions_.ShowItemToolTips = true;

            sessions_.KeyDown += SessionsKeyDown;
        }


        private void RobotGrapherKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                plotwindows_.DeleteSelected();
            else if (e.KeyCode == Keys.Insert)
                plotwindows_.AddGraph(datamgr_, "time");
            else if (e.KeyCode == Keys.Escape)
                plotwindows_.UnselectGraph();
        }

        private void SessionMenuClosed(object sender, EventArgs e)
        {
            activesession_ = string.Empty;
        }


        private void SessionContextMenuRequest(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = sessions_.HitTest(e.Location);
                if (info.Item != null)
                {
                    activesession_ = info.Item.Text;
                    sessionmenu_.Show(this, e.Location);
                }

            }
        }

        #endregion

        #region GUI Event Handlers

        private void DragVariableItem(object sender, ItemDragEventArgs e)
        {
            if (sessions_.SelectedItems.Count != 1)
                return;

            string session = GetSessionName(sessions_.SelectedItems[0].Text);
            if (variables_.SelectedItems.Count > 0)
            {
                string varlist = session;

                foreach (ListViewItem item in variables_.SelectedItems)
                {
                    varlist += ",";
                    varlist += item.Text;
                }

                variables_.DoDragDrop(varlist, DragDropEffects.Copy);
            }
        }

        private void SelectedSessionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (sessions_.SelectedItems.Count != 1)
                return;

            string name = GetSessionName(sessions_.SelectedItems[0].Text);

            variables_.Items.Clear();
            DataSession session = datamgr_[name];
            if (session != null)
            {
                IList<string> colnames = session.ColumnNames;
                foreach (string col in colnames)
                    variables_.Items.Add(col);
            }
        }

        private void NewSessionStarted(DataSession session)
        {
            bool found = false;

            foreach(ListViewItem item in sessions_.Items)
            {
                string sname = GetSessionName(item.Text);
                if (sname == session.Name)
                {
                    if (!item.Text.EndsWith(ActiveString))
                        item.Text += ActiveString;
                    item.ForeColor = Color.Black;
                    found = true;
                }
            }
            if (!found)
                sessions_.Items.Add(session.Name + ActiveString);
        }

        private void SessionEnded(DataSession session)
        {
            foreach (ListViewItem item in sessions_.Items)
            {
                if (item.Text.EndsWith(ActiveString))
                {
                    string sessname = item.Text.Substring(0, item.Text.Length - ActiveString.Length);
                    if (sessname == session.Name)
                    {
                        double pcnt = session.PercentDataPresent;
                        item.Text = sessname;
                        if (pcnt < 90.0)
                            item.ForeColor = Color.Red;

                        item.ToolTipText = "Data Present: " + pcnt.ToString() + "%\nSession ID: " + session.ID.ToString();

                        break;
                    }
                }
            }
        }

        private void NewSessionStarted(object sender, DataManagerEventArgs e)
        {
            if (InvokeRequired)
            {
                DataSessionDelegate d = new DataSessionDelegate(NewSessionStarted);
                Invoke(d, new object[] { e.Session });
            }
            else
                NewSessionStarted(e.Session);
        }

        private void SessionFinished(object sender, DataManagerEventArgs e)
        {
            if (InvokeRequired)
            {
                DataSessionDelegate d = new DataSessionDelegate(SessionEnded);
                Invoke(d, new object[] { e.Session });
            }
            else
                SessionEnded(e.Session);
        }
        #endregion

        #region overridden form methods
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            datamgr_.End();
        }

        #endregion

        #region menu handlers
        private void CreateNewGraphHandler(object sender, EventArgs e)
        {
            plotwindows_.AddGraph(datamgr_, "time");
        }

        private void SaveConfigMenuItemHandler(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Configuration Files (*.pcg)|*.pcg|All Files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<GraphConfig> configs = plotwindows_.Configs;
                string json = JsonConvert.SerializeObject(configs, Formatting.Indented);

                using (StreamWriter writer = new StreamWriter(dialog.FileName))
                {
                    writer.Write(json);
                }
            }
        }

        private void LoadConfigMenuItemHandler(object sender, EventArgs e)
        {
            List<GraphConfig> configs = null;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Configuration Files (*.pcg)|*.pcg|All Files (*.*)|*.*";
            dialog.Title = "Select Plotter Configuration File";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader rdr = new StreamReader(dialog.FileName))
                {
                    string json = rdr.ReadToEnd();
                    configs = JsonConvert.DeserializeObject<List<GraphConfig>>(json);
                }
            }

            if (configs == null)
                return;

            plotwindows_.Clear();
            plotwindows_.AddGraphs(datamgr_, configs);

        }

        private void SessionSaveMenuHandler(object sender, EventArgs e)
        {
            if (datamgr_.HasSession(activesession_))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Configuration Files (*.csv)|*.csv|All Files (*.*)|*.*";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DataSession session = datamgr_[activesession_];
                    session.WriteToFile(dialog.FileName);
                }
            }
        }

        private void SessionDeleteMenuHandler(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(activesession_))
            {
                DataSession session = datamgr_[activesession_];
                if (session != null)
                    RemoveDataSession(session);
            }
        }


        private void SessionsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            foreach (ListViewItem item in sessions_.SelectedItems)
            {
                DataSession session = datamgr_[GetSessionName(item.Text)];
                if (session != null)
                    RemoveDataSession(session);
            }
        }

        private void RemoveDataSession(DataSession session)
        {
            plotwindows_.RemoveIfSession(session);
            datamgr_.RemoveSession(session);

            foreach (ListViewItem item in sessions_.Items)
            {
                string sname = GetSessionName(item.Text);
                if (sname == session.Name)
                {
                    sessions_.Items.Remove(item);
                    break;
                }
            }
        }

        private void SessionViewMenuHandler(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(activesession_))
            {
                DataViewer viewer = new DataViewer();
                viewer.Session = datamgr_[activesession_];
                viewer.Show();
            }
        }

        private void DeleteMenuItemHandler(object sender, EventArgs e)
        {
            plotwindows_.DeleteSelected();
        }

        private void HelpAboutMenuHandler(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog();
        }
        #endregion

        #region private methods
        private string GetSessionName(string name)
        {
            if (name.EndsWith(ActiveString))
                name = name.Substring(0, name.Length - ActiveString.Length);

            return name;
        }
        #endregion
    }
}
