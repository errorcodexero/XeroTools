using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataPlotter
{
    [JsonObject(MemberSerialization.OptIn)]
    class GraphConfig
    {
        #region public constructor
        public GraphConfig(string title, string xvar)
        {
            title_ = title;
            variables_ = new List<GraphVariable>();
            xvariable_ = xvar;
        }

        public GraphConfig(string xvar)
        {
            title_ = string.Empty;
            variables_ =new List<GraphVariable>();
            xvariable_ = xvar;
        }
        
        public GraphConfig()
        {
            title_ = string.Empty;
            variables_ = new List<GraphVariable>();
            xvariable_ = string.Empty;
        }
        #endregion

        #region public properties
        [JsonProperty]
        public string Title
        {
            get { return title_; }
            set { title_ = value; }
        }

        [JsonProperty]
        public string XVariable
        {
            get { return xvariable_; }
            set { xvariable_ = value; }
        }

        [JsonProperty]
        public List<GraphVariable> Variables
        {
            get { return variables_; }
        }

        public string SessionName
        {
            get
            {
                string sname = string.Empty;

                foreach(GraphVariable v in variables_)
                {
                    if (string.IsNullOrEmpty(sname))
                    {
                        sname = v.SessionName;
                    }
                    else
                    {
                        if (sname != v.SessionName)
                            throw new Exception("invalid graph config, multiple sessions in one graph");
                    }
                }

                return sname;
            }
        }

        public DataSession Session
        {
            get
            {
                DataSession s = null;

                foreach (GraphVariable v in variables_)
                {
                    s = v.Session;
                    break;
                }
                return s;
            }
        }
        #endregion

        #region public methods
        public void AddVariable(GraphVariable gvar)
        {
            variables_.Add(gvar);
        }

        public bool ResolveVariables(DataSession session)
        {
            bool ret = false;

            foreach(GraphVariable v in variables_)
            {
                if (!v.IsResolved && v.SessionName == session.Name)
                {
                    v.Session = session;
                    ret = true;
                }
            }

            return ret;
        }
        #endregion

        #region private members
        private string title_;
        private List<GraphVariable> variables_;
        private string xvariable_;
        #endregion
    }
}
