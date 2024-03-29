﻿using System;
using Newtonsoft.Json;


namespace PathViewer
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PathFile
    {
        [JsonProperty(PropertyName = "pathdir")]
        public string PathDirectory;

        [JsonProperty(PropertyName = "robot")]
        public RobotParams Robot;

        [JsonProperty(PropertyName = "groups")]
        public PathGroup[] Groups;

        private bool m_dirty;
        private string m_filename;

        public PathFile()
        {
            Groups = new PathGroup[0];
            Robot = new RobotParams(RobotParams.TankDriveType, "inches", "degrees", 0.02, 24.0, 32.0, 96.0, 96.0, 96.0);
        }

        public PathFile(PathFile pf)
        {
            Robot = new RobotParams(pf.Robot);
            Groups = new PathGroup[pf.Groups.Length];

            for (int i = 0; i < Groups.Length; i++)
            {
                Groups[i] = new PathGroup(pf.Groups[i]);
            }

            IsDirty = true;
            m_filename = pf.m_filename;
        }

        public bool AllSegmentsReady
        {
            get
            {
                foreach(PathGroup gr in Groups)
                {
                    foreach(RobotPath path in gr.Paths)
                    {
                        if (!path.HasSegments || !path.HasAdditionalSegments)
                            return false;
                    }
                }
                return true;
            }

        }
        public int TotalPaths
        {
            get
            {
                int total = 0;

                foreach (PathGroup gr in Groups)
                    total += gr.Paths.Length;

                return total;
            }
        }

        public bool IsDirty
        {
            get { return m_dirty; }
            set { m_dirty = value; }
        }

        public string PathName
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public void UpdateMaxVelocity(double d)
        {
            foreach (PathGroup gr in Groups)
                gr.UpdateMaxVelocity(Robot.MaxVelocity, d);
            Robot.MaxVelocity = d;
        }

        public void UpdateMaxAcceleration(double d)
        {
            foreach (PathGroup gr in Groups)
                gr.UpdateMaxAcceleration(Robot.MaxAcceleration, d);
            Robot.MaxAcceleration = d;
        }

        public void UpdateMaxJerk(double d)
        {
            foreach (PathGroup gr in Groups)
                gr.UpdateMaxJerk(Robot.MaxJerk, d);
            Robot.MaxJerk = d;
        }

        public bool Contains(PathGroup group)
        {
            return Array.IndexOf(Groups, group) != -1;
        }

        public bool Contains(RobotPath path)
        {
            foreach(PathGroup group in Groups)
            {
                if (group.Contains(path))
                    return true;
            }

            return false;
        }

        public void ConvertUnits(string newunits)
        {
            if (newunits != Robot.LengthUnits)
            {
                string oldunits = Robot.LengthUnits;
                Robot.LengthUnits = newunits;
                m_dirty = true;
                Robot.ConvertUnits(oldunits, newunits);
                foreach (PathGroup gr in Groups)
                {
                    foreach (RobotPath path in gr.Paths)
                    {
                        path.ConvertUnits(oldunits, newunits);
                    }
                }

            }
        }

        public void RemoveGroup(string group)
        {
            int index = -1;

            for (int i = 0; i < Groups.Length; i++)
            {
                if (Groups[i].Name == group)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                m_dirty = true;
                PathGroup[] temp = new PathGroup[Groups.Length - 1];
                if (index > 0)
                    Array.Copy(Groups, 0, temp, 0, index);

                if (index < Groups.Length - 1)
                    Array.Copy(Groups, index + 1, temp, index, Groups.Length - index - 1);

                Groups = temp;
            }
        }

        public void RemovePath(string grname, string pathname)
        {
            PathGroup gr = FindGroupByName(grname);
            if (gr == null)
                return;

            gr.RemovePath(pathname);
            m_dirty = true;
        }

        public PathGroup FindGroupByPath(RobotPath p)
        {
            foreach (PathGroup gr in Groups)
            {
                foreach(RobotPath pa in gr.Paths)
                {
                    if (pa == p)
                        return gr;
                }
            }

            return null;
        }

        public PathGroup FindGroupByName(string name)
        {
            foreach (PathGroup gr in Groups)
            {
                if (gr.Name == name)
                    return gr;
            }

            return null;
        }

        public RobotPath FindPathByName(string group, string path)
        {
            PathGroup gr = FindGroupByName(group);
            if (gr == null)
                return null;

            return gr.FindPathByName(path);
        }

        public void AddPathGroup(string name)
        {
            PathGroup group = new PathGroup(name);
            Array.Resize<PathGroup>(ref Groups, Groups.Length + 1);
            Groups[Groups.Length - 1] = group;
            m_dirty = true;
        }

        public void AddPath(string group, string path)
        {
            PathGroup gr = FindGroupByName(group);
            if (gr == null)
                return;

            m_dirty = true;
            gr.AddPath(Robot, path);
        }

        public void AddPath(string group, RobotPath path)
        {
            PathGroup gr = FindGroupByName(group);
            if (gr == null)
                return;

            m_dirty = true;
            gr.AddPath(path);
        }

        public bool RenameGroup(string oldname, string newname)
        {
            PathGroup gr = FindGroupByName(oldname);
            if (gr == null)
                return false;

            gr.Name = newname;
            m_dirty = true;
            return true;
        }

        public bool RenamePath(string group, string oldname, string newname)
        {
            PathGroup gr = FindGroupByName(group);
            if (gr == null)
                return false;

            if (!gr.RenamePath(oldname, newname))
                return false;

            m_dirty = true;
            return true;
        }

        public bool FindPathByWaypoint(WayPoint pt, out PathGroup group, out RobotPath path)
        {
            group = null;
            path = null;

            foreach (PathGroup gr in Groups)
            {
                foreach (RobotPath pa in gr.Paths)
                {
                    foreach (WayPoint wy in pa.Points)
                    {
                        if (wy == pt)
                        {
                            group = gr;
                            path = pa;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool FindPathByWaypoint(WayPoint pt, out PathGroup group, out RobotPath path, out int index)
        {
            group = null;
            path = null;
            index = -1;

            foreach (PathGroup gr in Groups)
            {
                foreach (RobotPath pa in gr.Paths)
                {
                    for (int i = 0; i < pa.Points.Length; i++)
                    {
                        if (pa.Points[i] == pt)
                        {
                            group = gr;
                            path = pa;
                            index = i;

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
