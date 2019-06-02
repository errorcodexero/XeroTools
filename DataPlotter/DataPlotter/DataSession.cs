using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace DataPlotter
{
    class DataSession
    {
        #region private member variables
        private string name_;
        private int id_;
        private List<string> columns_;
        private List<double[]> rows_;
        private List<bool[]> present_;
        private bool finished_;
        #endregion

        #region public events
        public event EventHandler<EventArgs> DataValueAdded;
        public event EventHandler<EventArgs> DataRowAdded;
        public event EventHandler<EventArgs> SessionReset;
        #endregion

        #region public constructors
        public DataSession(string name, int id, string [] cols)
        {
            name_ = name;
            id_ = id;
            finished_ = false;
            columns_ = new List<string>(cols);
            rows_ = new List<double[]>();
            present_ = new List<bool[]>();
        }
        #endregion

        #region public properites
        public string Name
        {
            get { return name_; }
        }

        public bool Finished
        {
            get { return finished_; }
            set { finished_ = value; }
        }

        public int ID
        {
            get { return id_; }
        }

        public int ColumnCount
        {
            get { return columns_.Count ; }
        }

        public int RowCount
        {
            get { return rows_.Count; }
        }

        public IList<string> ColumnNames
        {
            get { return columns_; }
        }

        public double PercentDataPresent
        {
            get
            {
                int total = rows_.Count * ColumnCount ;
                int count = 0;
                foreach (bool [] pre in present_)
                {
                    foreach(bool b in pre)
                    {
                        if (b)
                            count++;
                    }
                }

                return (double)count / (double)total * 100.0;
            }
        }
        #endregion

        #region public methods
        public void WriteToFile(string filename)
        {
            using (StreamWriter strm = new StreamWriter(filename))
            {
                for (int i = 0; i < columns_.Count; i++)
                {
                    if (i != 0)
                        strm.Write(",");
                    strm.Write(columns_[i]);
                }
                strm.WriteLine();

                foreach (double[] row in rows_)
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        if (i != 0)
                            strm.Write(",");
                        strm.Write(row[i].ToString());
                    }
                    strm.WriteLine();
                }
            }
        }

        public bool Reset(int id, string[] cols)
        {
            if (id_ == id)
                return false;

            id_ = id;
            columns_.Clear();
            columns_.AddRange(cols);
            finished_ = false;
            rows_.Clear();
            present_.Clear();

            OnSessionReset(EventArgs.Empty);
            return true;
        }

        public double GetValue(int row, int col)
        {
            double[] rowvalues = rows_[row];
            return rowvalues[col];
        }

        public bool IsPresent(int row, int col)
        {
            bool[] presentdata = present_[row];
            return presentdata[col];
        }

        public int GetColumnFromName(string name)
        {
            return columns_.FindIndex(x => x.Equals(name));
        }

        public void AddData(int row, int col, double value)
        {
            double[] rowdata;
            bool[] presentdata;

            while (rows_.Count <= row)
            {
                //
                // Add a rows to the data sessions until we have the row
                // needed for the current data
                //
                rowdata = new double[ColumnCount];
                rows_.Add(rowdata);

                presentdata = new bool[ColumnCount];
                present_.Add(presentdata);
            }

            rowdata = rows_[row];
            rowdata[col] = value;

            presentdata = present_[row];
            presentdata[col] = true;

            OnDataValueAdded(EventArgs.Empty);


            if (Array.TrueForAll<bool>(presentdata, x => x))
                OnDataRowAdded(EventArgs.Empty);
        }
        #endregion

        #region protected methods
        protected virtual void OnDataValueAdded(EventArgs e)
        {
            DataValueAdded?.Invoke(this, e);
        }

        protected virtual void OnDataRowAdded(EventArgs e)
        {
            DataRowAdded?.Invoke(this, e);
        }

        protected virtual void OnSessionReset(EventArgs e)
        {
            SessionReset?.Invoke(this, e);
        }
        #endregion

        #region private methods
        private int AddColumn(string name)
        {
            int col = columns_.Count;
            columns_.Add(name);
            return col;
        }
        #endregion
    }
}
