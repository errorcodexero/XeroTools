using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataPlotter
{
    [JsonObject(MemberSerialization.OptIn)]
    class GraphVariable
    {
        #region private variables
        private string sessionname_;
        private DataSession session_;
        private object tag_;
        private int xcolumn_;
        private int ycolumn_;
        private string xvar_;
        private string yvar_;
        #endregion

        #region public constructor
        public GraphVariable(DataSession session, string xname, string yname)
        {
            Session = session;
            xvar_ = xname;
            yvar_ = yname;

            if (session != null)
            {
                sessionname_ = session.Name;
                xcolumn_ = session.GetColumnFromName(xvar_);
                ycolumn_ = session.GetColumnFromName(yvar_);
            }
            else
            {
                sessionname_ = string.Empty;
                xcolumn_ = -1;
                ycolumn_ = -1;
            }
        }
        #endregion

        #region public properties
        [JsonProperty]
        public string XVariable
        {
            get { return xvar_; }
            set { xvar_ = value; }
        }

        [JsonProperty]
        public string YVariable
        {
            get { return yvar_; }
            set { yvar_ = value; }
        }

        public object Tag
        {
            get { return tag_; }
            set { tag_ = value; }
        }

        public DataSession Session
        {
            get { return session_; }
            set
            {
                session_ = value;
                if (session_ != null)
                {
                    if (xcolumn_ == -1)
                        xcolumn_ = session_.GetColumnFromName(xvar_);
                    if (ycolumn_ == -1)
                        ycolumn_ = session_.GetColumnFromName(yvar_);
                }
            }
        }

        public int XColumn
        {
            get { return xcolumn_; }
            set { xcolumn_ = value; }
        }

        public int YColumn
        {
            get { return ycolumn_; }
            set { ycolumn_ = value; }
        }

        [JsonProperty]
        public string SessionName
        {
            get { return sessionname_; }
            set { sessionname_ = value; }
        }

        public bool IsResolved
        {
            get { return session_ != null;  }
        }
        #endregion
    }
}
