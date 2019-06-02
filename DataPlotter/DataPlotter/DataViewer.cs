using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataPlotter
{
    partial class DataViewer : Form
    {
        DataSession session_;

        public DataViewer()
        {
            InitializeComponent();
        }

        public DataSession Session
        {
            get { return session_; }
            set
            {
                session_ = value;
                UpdateViewer();
            }
        }

        private void UpdateViewer()
        {
            datalist_.Columns.Clear();
            datalist_.Items.Clear();

            foreach(string col in session_.ColumnNames)
            {
                datalist_.Columns.Add(col);
            }

            for(int row = 0; row < session_.RowCount; row++)
            {
                ListViewItem item = new ListViewItem(session_.GetValue(row, 0).ToString());
                for(int col = 1; col < session_.ColumnCount; col++)
                {
                    item.SubItems.Add(session_.GetValue(row, col).ToString());
                }
                datalist_.Items.Add(item);
            }

            for(int i = 0; i < datalist_.Columns.Count; i++)
                datalist_.Columns[i].Width = -1;
        }
    }
}
