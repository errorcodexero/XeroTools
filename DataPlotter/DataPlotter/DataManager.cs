using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace DataPlotter
{
    class DataManager
    {
        #region private member variables
        private object session_lock_;
        private Dictionary<Thread, DataListener> listeners_;
        private bool running_;
        private Dictionary<string, DataSession> sessions_ ;
        #endregion

        #region public events
        public event EventHandler<DataManagerEventArgs> SessionStarted;
        public event EventHandler<DataManagerEventArgs> SessionFinished;
        #endregion

        #region public constructors
        public DataManager()
        {
            listeners_ = new Dictionary<Thread, DataListener>();
            running_ = true;
            session_lock_ = new object();
            sessions_ = new Dictionary<string, DataSession>();
        }
        #endregion

        #region public properties
        public IList<string> SessionNames
        {
            get
            {
                List<string> ret;
                lock(session_lock_)
                {
                    ret = new List<string>(sessions_.Keys);
                }

                return ret;
            }
        }

        public DataSession this[string name]
        {
            get
            {
                DataSession ret = null;

                lock(session_lock_)
                {
                    if (sessions_.ContainsKey(name))
                        ret = sessions_[name];
                }

                return ret;
            }
        }

        #endregion

        #region public methods
        public bool HasSession(string name)
        {
            return sessions_.ContainsKey(name);
        }

        public bool HasSession(int id)
        {
            foreach (DataSession session in sessions_.Values)
                if (session.ID == id)
                    return true;

            return false;
        }

        public void Start()
        {
            DataListener listener;
            Thread th;

            listener = new UDPDataListener(5555);
            th = new Thread(MonitorListener);
            th.Start(listener);

            listeners_[th] = listener;
        }

        public void End()
        {
            running_ = false;

            foreach (Thread th in listeners_.Keys)
            {
                DataListener listener = listeners_[th];
                listener.ShutDown();
                th.Join();
            }
        }

        public void RemoveSession(DataSession s)
        {
            sessions_.Remove(s.Name);
        }
        #endregion

        #region protected methods
        protected virtual void OnDataSessionStarted(DataManagerEventArgs e)
        {
            SessionStarted?.Invoke(this, e);
        }

        protected virtual void OnDataSessionFinished(DataManagerEventArgs e)
        {
            SessionFinished?.Invoke(this, e);
        }
        #endregion

        #region private member constants
        const string startkeyword = "$start,";
        const string datakeyword = "$data,";
        const string endkeyword = "$end,";
        #endregion

        #region private methods
        private DataSession GetSessionByID(int id)
        {
            foreach (DataSession s in sessions_.Values)
                if (s.ID == id)
                    return s;

            return null;
        }

        private bool ParseStartLine(string line, out string name, out int id, out string[] columns)
        {
            int index;
            string tmp;

            name = "";
            columns = null;
            id = -1;

            //
            // Remove the data keyword
            //
            line = line.Substring(startkeyword.Length);
            line = line.Substring(0, line.Length - 1);          // Remove the trailing $

            //
            // Extract the name of the plot
            //
            index = line.IndexOf(',');
            if (index == -1)
                return false;
            name = line.Substring(0, index);
            line = line.Substring(name.Length + 1);
            if (line.Length < 1)
                return false;

            //
            // Extract the row of the plot data
            //
            index = line.IndexOf(',');
            if (index == -1)
                return false;
            tmp = line.Substring(0, index);
            line = line.Substring(tmp.Length + 1);
            if (line.Length < 1)
                return false;
            if (!Int32.TryParse(tmp, out id))
                return false;

            columns = line.Split(',');
            return true;
        }

        private bool ParseDataLine(string line, out int id, out int row, out int col, out double value)
        {
            int index;
            string tmp;

            id = -1;
            row = -1;
            col = -1;
            value = 0.0;

            //
            // Remove the data keyword
            //
            line = line.Substring(datakeyword.Length);

            //
            // Extract the name of the plot
            //
            index = line.IndexOf(',');
            if (index == -1)
                return false;
            tmp = line.Substring(0, index);
            line = line.Substring(tmp.Length + 1);
            if (line.Length < 1)
                return false;
            if (!Int32.TryParse(tmp, out id))
                return false;


            //
            // Extract the row of the plot data
            //
            index = line.IndexOf(',');
            if (index == -1)
                return false;
            tmp = line.Substring(0, index);
            line = line.Substring(tmp.Length + 1);
            if (line.Length < 1)
                return false;
            if (!Int32.TryParse(tmp, out row))
                return false;


            //
            // Extract the name of the variable
            //
            index = line.IndexOf(',');
            if (index == -1)
                return false;
            tmp = line.Substring(0, index);
            line = line.Substring(tmp.Length + 1);
            if (line.Length < 1)
                return false;
            if (!Int32.TryParse(tmp, out col))
                return false;

            //
            // Extract the row of the plot data
            //
            index = line.IndexOf('$');
            if (index == -1)
                return false;
            tmp = line.Substring(0, index);
            if (!Double.TryParse(tmp, out value))
                return false;

            return true;
        }

        private void MonitorListener(object listobj)
        {
            
            DataListener listener = listobj as DataListener;
            while (running_)
            {
                string resp = listener.WaitForData();
                if (resp.StartsWith(startkeyword))
                {
                    string name;
                    string [] cols;
                    int id;
                    DataSession session;

                    if (!ParseStartLine(resp, out name, out id, out cols))
                        return;

                    lock (session_lock_)
                    {
                        if (sessions_.ContainsKey(name))
                        {
                            session = sessions_[name];
                            if (!session.Reset(id, cols))
                                session = null;
                        }
                        else
                        {
                            session = new DataSession(name, id, cols);
                            sessions_[name] = session;
                        }
                    }
                    if (session != null)
                        OnDataSessionStarted(new DataManagerEventArgs(session));
                }
                else if (resp.StartsWith(datakeyword))
                {
                    int id;
                    int col;
                    int row;
                    double value;
                    DataSession session = null;

                    if (!ParseDataLine(resp, out id, out row, out col, out value))
                        return;

                    lock(session_lock_)
                    {
                        session = GetSessionByID(id);
                    }

                    if (session != null)
                        session.AddData(row, col, value);
                }
                else if (resp.StartsWith(endkeyword))
                {
                    resp = resp.Substring(endkeyword.Length);
                    resp = resp.Substring(0, resp.Length - 1);
                    int id;

                    if (!Int32.TryParse(resp, out id))
                        return;

                    DataSession session = null;

                    lock (session_lock_)
                    {
                        session = GetSessionByID(id);
                        if (session != null)
                            session.Finished = true;
                    }

                    if (session != null)
                        OnDataSessionFinished(new DataManagerEventArgs(session));
                }
                else
                {
                    //
                    // We got bad data, not understood.  Ignore for now.  In the future report
                    // it somehow
                    //
                }
            }
        }
        #endregion

    }
}
