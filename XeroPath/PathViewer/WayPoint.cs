﻿using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PathViewer
{
    [DebuggerDisplay("X={X} Y={Y} Heading={Heading} Velocity={Velocity}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class WayPoint
    {
        [JsonProperty(PropertyName = "X")]
        public double X;

        [JsonProperty(PropertyName = "Y")]
        public double Y;
       
        [JsonProperty(PropertyName = "Heading")]
        public double Heading;

        [JsonProperty(PropertyName = "Velocity")]
        public double Velocity;

        public WayPoint()
        {
            X = 0.0;
            Y = 0.0;
            Heading = 0.0;
            Velocity = 0.0;
        }

        public WayPoint(double x, double y, double h, double v)
        {
            X = x;
            Y = y;
            Heading = h;
            Velocity = v;
        }

        public WayPoint(WayPoint other)
        {
            X = other.X;
            Y = other.Y;
            Heading = other.Heading;
            Velocity = other.Velocity;
        }

        public double Distance(WayPoint other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public void Update(double x, double y, double h, double v)
        {
            X = x;
            Y = y;
            Heading = h;
            Velocity = v;
        }
    }
}
