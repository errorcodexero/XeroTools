﻿using System.Collections.Generic;

namespace PathViewer
{
    public class PathSegment
    {
        #region private member variables
        private Dictionary<string, double> m_values;
        #endregion

        #region public constructor
        public PathSegment()
        {
            m_values = new Dictionary<string, double>();
        }

        public PathSegment(PathSegment seg)
        {
            m_values = new Dictionary<string, double>();
            foreach (var pair in seg.m_values)
                SetValue(pair.Key, pair.Value);
        }
        #endregion

        #region public properties
        public IEnumerable<string> ValueNames
        {
            get { return m_values.Keys; }
        }

        public int Count
        {
            get { return m_values.Count; }
        }
        #endregion

        #region public methods
        public void SetValue(string name, double v)
        {
            m_values[name] = v;
        }

        public double GetValue(string name)
        {
            return m_values[name];
        }

        public bool HasValue(string name)
        {
            return m_values.ContainsKey(name);
        }
        #endregion
    }
}
