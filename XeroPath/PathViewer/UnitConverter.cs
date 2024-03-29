﻿using System;
using System.Collections.Generic;

namespace PathViewer
{
    public class UnitConverter
    {
        private struct Units
        {
            public List<string> UnitNames;
            public double ToInches;

            public Units(List<string> names, double to)
            {
                UnitNames = names;
                ToInches = to;
            }
        };

        private static Units[] UnitsEntries =
        {
            new Units(new List<string>(){"inches", "in", "inch"}, 1.0),
            new Units(new List<string>(){"feet", "foot", "ft"}, 12.0),
            new Units(new List<string>(){"cm"}, 0.393701),
            new Units(new List<string>(){"meters", "meter", "m"}, 39.3701),
        };

        public static List<string> SupportedUnits
        {
            get
            {
                List<string> ret = new List<string>();
                for (int i = 0; i < UnitsEntries.Length; i++)
                {
                    ret.Add(UnitsEntries[i].UnitNames[0]);
                }
                return ret;
            }
        }


        public static double Convert(double inval, string from, string to)
        {
            bool found = false;
            double v = inval;

            found = false;
            for (int i = 0; i < UnitsEntries.Length && !found; i++)
            {
                if (UnitsEntries[i].UnitNames.Contains(from))
                {
                    found = true;
                    v *= UnitsEntries[i].ToInches;
                }
            }

            if (!found)
                throw new Exception("UnitsConverter - from units '" + from + "'not known");

            found = false;
            for(int i = 0; i < UnitsEntries.Length && !found; i++)
            {
                if (UnitsEntries[i].UnitNames.Contains(to))
                {
                    found = true;
                    v /= UnitsEntries[i].ToInches;
                }
            }

            if (!found)
                throw new Exception("UnitsConverter - to units '" + to + "'not known");

            return v;
        }
    }
}
