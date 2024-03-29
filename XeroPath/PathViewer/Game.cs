﻿using System;
using System.Drawing;
using Newtonsoft.Json;

namespace PathViewer
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Game
    {
        #region events
        public event EventHandler<EventArgs> UnitsChanged;

        #endregion

        #region private members

        private string m_path;

        private string m_file;

        #endregion

        public class Corners
        {
            [JsonProperty(PropertyName = "top-left")]
            public readonly double [] TopLeft ;

            [JsonProperty(PropertyName = "bottom-right")]
            public readonly double[] BottomRight;

            public Corners()
            {
                TopLeft = new double[2];
                BottomRight = new double[2];
            }
        }

        [JsonProperty(PropertyName ="game")]
        public readonly string Name;

        [JsonProperty(PropertyName = "field-image")]
        public readonly string FieldImage;

        [JsonProperty(PropertyName = "field-size")]
        public readonly double[] FieldSize;

        [JsonProperty(PropertyName = "field-corners")]
        public readonly Corners FieldCorners;

        [JsonProperty(PropertyName = "field-unit")]
        public string FieldUnits;

        public Game()
        {
            FieldUnits = "foot";
            FieldSize = new double[2];
            FieldCorners = new Corners();
        }

        public Game(string n, string i, SizeF s, Rectangle c)
        {
            Name = n;
            FieldUnits = "foot";
            FieldSize = new double[2];
            FieldCorners = new Corners();
            FieldSize[0] = s.Width;
            FieldSize[1] = s.Height;
        }

        /// <summary>
        /// This property is the scale factor from field related distances to Image pixel distances.
        /// </summary>
        public double ScaleFactorWorld2Image
        {
            get
            {
                return (float)(FieldCorners.BottomRight[0] - FieldCorners.TopLeft[0]) / (float)(FieldSize[0]);
            }
        }

        public string Units
        {
            get { return FieldUnits; }
            set
            {
                FieldSize[0] = UnitConverter.Convert(FieldSize[0], FieldUnits, value);
                FieldSize[1] = UnitConverter.Convert(FieldSize[1], FieldUnits, value);
                FieldUnits = value;
                OnUnitsChanged(EventArgs.Empty);
            }
        }

        public PointF Origin
        {
            get
            {
                double x = (float)FieldCorners.TopLeft[0] / ScaleFactorWorld2Image;
                double y = (float)FieldCorners.BottomRight[1] / ScaleFactorWorld2Image;

                return new PointF((float)x, (float)y);
            }
        }

        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        public string File
        {
            get { return m_file; }
            set { m_file = value; }
        }

        public string ImagePath
        {
            get
            {
                return System.IO.Path.Combine(m_path, FieldImage);
            }
        }

        public int Year
        {
            get
            {
                int ret = 0;

                int index = 0;

                while (index < m_file.Length && Char.IsDigit(m_file[index]))
                    index++;

                if (!Int32.TryParse(m_file.Substring(0, index), out ret))
                    ret = 0;

                return ret;
            }
        }

        protected virtual void OnUnitsChanged(EventArgs args)
        {
            EventHandler<EventArgs> handler = UnitsChanged;
            handler?.Invoke(this, args);
        }

    }
}
